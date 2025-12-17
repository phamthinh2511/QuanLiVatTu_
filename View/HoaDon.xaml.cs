using PageNavigation.Model;
using PageNavigation.View.PopupDetail;
using PageNavigation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PageNavigation.View
{
    /// <summary>
    /// Interaction logic for HoaDon.xaml
    /// </summary>
    public partial class HoaDon : UserControl
    {
        public HoaDon()
        {
            InitializeComponent();
            // KẾT NỐI VIEWMODEL (Để load danh sách lên ListView)
            this.DataContext = new HoaDonVM();
        }

        // --- 1. NÚT TẠO MỚI ---
        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            // Mở popup với tham số null = Tạo mới
            HoaDonDetail popup = new HoaDonDetail(null);
            if (popup.ShowDialog() == true)
            {
                RefreshData(); // Tải lại danh sách sau khi lưu
            }
        }

        // --- 2. NÚT XEM / SỬA ---
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lvHoaDon.SelectedItem as HoaDonM;
            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!", "Thông báo");
                return;
            }

            HoaDonDetail popup = new HoaDonDetail(selectedItem);
            if (popup.ShowDialog() == true)
            {
                RefreshData();
            }
        }

        // --- 3. NÚT XÓA (Logic an toàn: Xóa con trước -> Xóa cha sau) ---
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lvHoaDon.SelectedItem as HoaDonM;
            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!", "Thông báo");
                return;
            }

            if (MessageBox.Show($"Bạn chắc chắn muốn xóa hóa đơn số {selectedItem.MaHoaDon}?\n(Hàng sẽ được hoàn trả lại vào kho)",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new QuanLyVatTuContext())
                    {
                        var hd = db.HoaDon.Find(selectedItem.MaHoaDon);
                        if (hd != null)
                        {
                            // 1. Lấy danh sách chi tiết của hóa đơn này
                            var chiTiet = db.CT_HoaDon.Where(x => x.MaHoaDon == hd.MaHoaDon).ToList();

                            // --- ĐOẠN MỚI THÊM: HOÀN TRẢ KHO ---
                            foreach (var item in chiTiet)
                            {
                                var vt = db.VatTu.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                                if (vt != null)
                                {
                                    // Trả lại số lượng đã bán vào kho
                                    vt.SoLuongTon = (vt.SoLuongTon ?? 0) + item.SoLuongBan;
                                }
                            }
                            // ------------------------------------

                            // 2. Xóa chi tiết (Sau khi đã cộng kho xong)
                            if (chiTiet.Count > 0)
                            {
                                db.CT_HoaDon.RemoveRange(chiTiet);
                            }

                            // 3. Xóa Hóa đơn
                            db.HoaDon.Remove(hd);

                            // 4. Lưu tất cả thay đổi
                            db.SaveChanges();

                            MessageBox.Show("Đã xóa hóa đơn và hoàn trả hàng vào kho!");
                            RefreshData();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                }
            }
        }

        // Hàm hỗ trợ gọi lại LoadData từ ViewModel
        private void RefreshData()
        {
            var vm = this.DataContext as HoaDonVM;
            if (vm != null)
            {
                vm.LoadData();
            }
        }
    }
}
