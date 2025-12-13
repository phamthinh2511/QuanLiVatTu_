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

            if (MessageBox.Show($"Bạn chắc chắn muốn xóa phiếu {selectedItem.MaPhieuNhap}?",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new QuanLyVatTuContext())
                    {
                        var phieu = db.PhieuNhapVatTu.Find(selectedItem.MaPhieuNhap);

                        if (phieu != null)
                        {
                            // --- BƯỚC 1: XÓA CHI TIẾT TRƯỚC (BẮT BUỘC) ---
                            // Lấy tất cả vật tư trong phiếu này ra
                            var chiTiet = db.CT_PhieuNhapVatTu
                                            .Where(x => x.MaPhieuNhap == phieu.MaPhieuNhap)
                                            .ToList();

                            // Nếu có chi tiết thì xóa hết đi
                            if (chiTiet.Count > 0)
                            {
                                db.CT_PhieuNhapVatTu.RemoveRange(chiTiet);
                            }

                            // --- BƯỚC 2: XÓA PHIẾU SAU ---
                            db.PhieuNhapVatTu.Remove(phieu);

                            // --- BƯỚC 3: LƯU DB ---
                            db.SaveChanges();

                            MessageBox.Show("Đã xóa thành công!");
                            RefreshData(); // Tải lại danh sách
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