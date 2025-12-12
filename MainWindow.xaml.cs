using PageNavigation.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PageNavigation
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Debug: Kiểm tra xem event có được trigger không
            System.Diagnostics.Debug.WriteLine("SelectionChanged triggered");

            if (listview.SelectedItem == null)
            {
                System.Diagnostics.Debug.WriteLine("SelectedItem is null");
                return;
            }

            var selectedItem = listview.SelectedItem as ListViewItem;
            var tag = selectedItem?.Tag?.ToString();

            System.Diagnostics.Debug.WriteLine($"Selected Tag: {tag}");

            var vm = this.DataContext as NavigationVM;
            if (vm == null)
            {
                System.Diagnostics.Debug.WriteLine("DataContext is null!");
                return;
            }

            System.Diagnostics.Debug.WriteLine("DataContext OK, executing command...");

            switch (tag)
            {
                case "Home":
                    if (vm.HomeCommand != null && vm.HomeCommand.CanExecute(null))
                        vm.HomeCommand.Execute(null);
                    else
                        System.Diagnostics.Debug.WriteLine("HomeCommand is null or cannot execute");
                    break;
                case "NhanVien":
                    if (vm.NhanVienCommand != null && vm.NhanVienCommand.CanExecute(null))
                        vm.NhanVienCommand.Execute(null);
                    break;
                case "LoaiVatTu":
                    if (vm.LoaiVatTuCommand != null && vm.LoaiVatTuCommand.CanExecute(null))
                        vm.LoaiVatTuCommand.Execute(null);
                    break;
                case "VatTu":
                    if (vm.VatTuCommand != null && vm.VatTuCommand.CanExecute(null))
                        vm.VatTuCommand.Execute(null);
                    break;
                case "KhachHang":
                    if (vm.KhachHangCommand != null && vm.KhachHangCommand.CanExecute(null))
                        vm.KhachHangCommand.Execute(null);
                    break;
                case "HoaDon":
                    if (vm.HoaDonCommand != null && vm.HoaDonCommand.CanExecute(null))
                        vm.HoaDonCommand.Execute(null);
                    break;
                case "PhieuNhapVatTu":
                    if (vm.PhieuNhapVatTuCommand != null && vm.PhieuNhapVatTuCommand.CanExecute(null))
                        vm.PhieuNhapVatTuCommand.Execute(null);
                    break;
                case "PhieuThuTien":
                    if (vm.PhieuThuTienCommand != null && vm.PhieuThuTienCommand.CanExecute(null))
                        vm.PhieuThuTienCommand.Execute(null);
                    break;
                case "TraCuu":
                    if (vm.TraCuuCommand != null && vm.TraCuuCommand.CanExecute(null))
                        vm.TraCuuCommand.Execute(null);
                    break;
                case "BaoCao":
                    if (vm.BaoCaoCommand != null && vm.BaoCaoCommand.CanExecute(null))
                        vm.BaoCaoCommand.Execute(null);
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine($"Unknown tag: {tag}");
                    break;
            }
        }

        private void togglebutton_Checked(object sender, RoutedEventArgs e)
        {
            Pages.Opacity = 0.3;
            Pages.IsEnabled = false;
        }

        private void togglebutton_Unchecked(object sender, RoutedEventArgs e)
        {
            Pages.Opacity = 1;
            Pages.IsEnabled = true;
        }

        private void closebutton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}