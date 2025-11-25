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
using PageNavigation.View.PopupDetail;
using PageNavigation.ViewModel;

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

        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedCustomer = CustomerListView.SelectedItem as KhachHangVM;
            if (selectedCustomer != null)
            {
                KhachHangDetail detailWindow = new KhachHangDetail(selectedCustomer);
                if (detailWindow.ShowDialog() == true)
                {
                    CustomerListView.Items.Refresh();
                }
            }
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as DanhSachKhachHangVM;
            if (viewModel == null) return;
            int newId = 1;

            var existingIds = viewModel.ListCustomers
                                .Select(c => c.CustomerID)
                                .OrderBy(id => id)
                                .ToList();
            foreach (int id in existingIds)
            {
                if (id == newId)
                {
                    newId++;
                }
                else if (id > newId)
                {
                    break;
                }
            }
            var newCustomer = new KhachHangVM();
            newCustomer.CustomerID = newId;
            newCustomer.CustomerBirth = DateOnly.FromDateTime(DateTime.Now);
            newCustomer.CustomerGender = "Nam";
            KhachHangDetail popup = new KhachHangDetail(newCustomer);
            if (popup.ShowDialog() == true)
            {
                viewModel.AddCustomer(newCustomer);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as DanhSachKhachHangVM;
            if (viewModel == null) return;
            var selectedCustomer = CustomerListView.SelectedItem as KhachHangVM;
            if (selectedCustomer == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng {selectedCustomer.CustomerName} (ID: {selectedCustomer.CustomerID})?",
                                         "Xác nhận xóa",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                viewModel.ListCustomers.Remove(selectedCustomer);
            }
        }
    } 
}
