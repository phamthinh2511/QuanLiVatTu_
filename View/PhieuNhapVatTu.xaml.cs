using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using PageNavigation.View.PopupDetail;
using PageNavigation.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PageNavigation.View
{
    public partial class PhieuNhapVatTu : UserControl
    {
        public PhieuNhapVatTu()
        {
            InitializeComponent();

            // Kết nối với ViewModel để load dữ liệu lên ListView
            // (Đảm bảo bạn đã có class PhieuNhapVatTuVM chứa ListPhieuNhap)
            this.DataContext = new PhieuNhapVatTuVM();
        }

        // --- 1. NÚT TẠO MỚI ---
        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            // Mở popup với tham số null (nghĩa là tạo mới)
            PhieuNhapVatTuDetail popup = new PhieuNhapVatTuDetail(null);

            // Nếu người dùng bấm Lưu (DialogResult == true) thì tải lại danh sách
            if (popup.ShowDialog() == true)
            {
                RefreshData();
            }
        }

        // --- 2. NÚT SỬA ---
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dòng đang chọn trên ListView
            var selectedItem = lvPhieuNhap.SelectedItem as PhieuNhapVatTuM;

            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu cần sửa!", "Thông báo");
                return;
            }

            // Mở popup và truyền phiếu đang chọn vào để hiển thị thông tin cũ
            PhieuNhapVatTuDetail popup = new PhieuNhapVatTuDetail(selectedItem);

            if (popup.ShowDialog() == true)
            {
                RefreshData();
            }
        }

        // --- 3. NÚT XÓA (Logic quan trọng) ---
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lvPhieuNhap.SelectedItem as PhieuNhapVatTuM;

            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu cần xóa!", "Thông báo");
                return;
            }

            if (MessageBox.Show($"Bạn chắc chắn muốn xóa phiếu nhập {selectedItem.MaPhieuNhap}?\n(Kho sẽ bị trừ đi số lượng tương ứng)",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new QuanLyVatTuContext())
                    {
                        var phieu = db.PhieuNhapVatTu.Find(selectedItem.MaPhieuNhap);

                        if (phieu != null)
                        {
                            // 1. Lấy danh sách chi tiết của phiếu này
                            var chiTiet = db.CT_PhieuNhapVatTu.Where(x => x.MaPhieuNhap == phieu.MaPhieuNhap).ToList();

                            // --- BƯỚC KIỂM TRA AN TOÀN (QUAN TRỌNG) ---
                            // Kiểm tra xem có đủ hàng trong kho để trừ không?
                            // (Tránh trường hợp: Nhập xong bán hết rồi giờ quay lại xóa phiếu nhập)
                            foreach (var item in chiTiet)
                            {
                                var vtCheck = db.VatTu.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                                if (vtCheck != null)
                                {
                                    if ((vtCheck.SoLuongTon ?? 0) < item.SoLuong)
                                    {
                                        MessageBox.Show($"Không thể xóa phiếu này!\n" +
                                                        $"Vật tư '{vtCheck.TenVatTu}' trong phiếu là {item.SoLuong}, " +
                                                        $"nhưng tồn kho hiện tại chỉ còn {vtCheck.SoLuongTon}.\n\n" +
                                                        "Lý do: Hàng này có thể đã bị bán hoặc xuất đi rồi.",
                                                        "Lỗi kho hàng", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return; // Dừng lại ngay, không cho xóa
                                    }
                                }
                            }
                            // -------------------------------------------

                            // --- NẾU ĐỦ ĐIỀU KIỆN THÌ MỚI TRỪ KHO ---
                            foreach (var item in chiTiet)
                            {
                                var vt = db.VatTu.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                                if (vt != null)
                                {
                                    // Trừ kho (Đảo ngược quá trình nhập)
                                    vt.SoLuongTon = (vt.SoLuongTon ?? 0) - item.SoLuong;
                                }
                            }

                            // 2. Xóa chi tiết
                            if (chiTiet.Count > 0)
                            {
                                db.CT_PhieuNhapVatTu.RemoveRange(chiTiet);
                            }

                            // 3. Xóa Phiếu cha
                            db.PhieuNhapVatTu.Remove(phieu);

                            // 4. Lưu tất cả
                            db.SaveChanges();

                            MessageBox.Show("Đã xóa phiếu nhập và cập nhật lại kho!");
                            RefreshData();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        // Hàm hỗ trợ tải lại dữ liệu từ ViewModel
        private void RefreshData()
        {
            // Gọi hàm LoadData trong ViewModel để cập nhật lại ListView
            // (Giả sử ViewModel của bạn có hàm LoadData hoặc LoadDataAsync)
            var vm = this.DataContext as PhieuNhapVatTuVM;
            if (vm != null)
            {
                // Nếu hàm LoadData là async, ta gọi thế này để chạy nền
                vm.LoadData();
            }
        }
    }
}