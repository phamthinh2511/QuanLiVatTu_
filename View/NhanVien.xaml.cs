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
    /// Interaction logic for NhanVien.xaml
    /// </summary>
    public partial class NhanVien : UserControl
    {
        public NhanVien()
        {
            InitializeComponent();
            DataContext = new NhanVienVM();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var popup = new NhanVienDetail();

            if (popup.ShowDialog() == true)
            {
                var vm = DataContext as NhanVienVM;
                if (vm != null)
                    vm.LoadDataAsync();
            }
            //if (popup.ShowDialog() == true)
            //{

            //    var viewModel = this.DataContext as NhanVienVM;
            //    if (viewModel != null)
            //    {
            //        viewModel.LoadDataAsync();
            //    }
            //}
        }

        private async void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            var popup = new NhanVienDetail();

            if (popup.ShowDialog() == true)
            {
                if (DataContext is NhanVienVM vm)
                {
                    await vm.LoadDataAsync();
                }
            }
        }

        private async void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            // LƯU Ý: Bạn cần thay 'EmployeeListView' bằng tên (x:Name) của ListView trong file XAML của bạn
            var selectedEmployee = EmployeeListView.SelectedItem as NhanVienM;

            if (selectedEmployee == null)
            {
                // Có thể hiện thông báo nhắc người dùng chọn dòng cần sửa
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Truyền nhân viên đã chọn vào Popup để hiển thị thông tin cũ
            var popup = new NhanVienDetail(selectedEmployee);

            // Nếu người dùng nhấn Lưu và đóng popup
            if (popup.ShowDialog() == true)
            {
                if (DataContext is NhanVienVM vm)
                {
                    await vm.LoadDataAsync(); // Tải lại danh sách
                }
            }
            else
            {
                // Trường hợp người dùng sửa xong nhưng đóng popup (không ấn Lưu) hoặc Cancel,
                // ta cũng nên reload lại để revert các thay đổi hiển thị trên UI (nếu có binding 2 chiều)
                if (DataContext is NhanVienVM vm)
                {
                    await vm.LoadDataAsync();
                }
            }
        }

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as NhanVienVM;
            if (vm == null) return;

            var selectedEmployee = EmployeeListView.SelectedItem as NhanVienM;
            if (selectedEmployee == null)
                return;

            
            vm.DeleteEmployee(selectedEmployee);

           
        }
    }
}
