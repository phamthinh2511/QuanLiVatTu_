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

            if (MessageBox.Show($"Bạn chắc chắn muốn xóa hóa đơn số {selectedItem.MaHoaDon}?",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new QuanLyVatTuContext())
                    {
                        var hd = db.HoaDon.Find(selectedItem.MaHoaDon);
                        if (hd != null)
                        {
                            // BƯỚC 1: Xóa sạch chi tiết hóa đơn (Con)
                            var chiTiet = db.CT_HoaDon.Where(x => x.MaHoaDon == hd.MaHoaDon).ToList();
                            if (chiTiet.Count > 0)
                            {
                                db.CT_HoaDon.RemoveRange(chiTiet);
                            }

                            // BƯỚC 2: Xóa hóa đơn (Cha)
                            db.HoaDon.Remove(hd);

                            // BƯỚC 3: Lưu DB
                            db.SaveChanges();

                            MessageBox.Show("Đã xóa thành công!");
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
