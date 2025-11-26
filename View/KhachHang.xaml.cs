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
    /// Interaction logic for KhachHang.xaml
    /// </summary>
    public partial class KhachHang : UserControl
    {
        public KhachHang()
        {
            InitializeComponent();
        }


        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {

            KhachHangDetail popup = new KhachHangDetail();

            if (popup.ShowDialog() == true)
            {
 
                var viewModel = this.DataContext as DanhSachKhachHangVM;
                if (viewModel != null)
                {
                    viewModel.LoadDataAsync();
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {

            var selectedCustomer = CustomerListView.SelectedItem as KhachHangM;

            if (selectedCustomer == null)
            {
                return;
            }

            KhachHangDetail popup = new KhachHangDetail(selectedCustomer);
            var result = popup.ShowDialog();

            var viewModel = this.DataContext as DanhSachKhachHangVM;
            if (viewModel != null)
            {
                viewModel.LoadDataAsync();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as DanhSachKhachHangVM;
            if (viewModel == null) return;

            var selectedCustomer = CustomerListView.SelectedItem as KhachHangM;

            if (selectedCustomer == null)
            {
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng {selectedCustomer.HoVaTen} (ID: {selectedCustomer.MaKhachHang})?",
                                         "Xác nhận xóa",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                viewModel.DeleteCustomer(selectedCustomer);
            }
        }
    } 
}
