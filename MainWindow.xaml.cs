using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PageNavigation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if(togglebutton.IsChecked == true)
            {
                tt_Home.Visibility = Visibility.Collapsed;
                tt_NhanVien.Visibility = Visibility.Collapsed;
                tt_LoaiVatTu.Visibility = Visibility.Collapsed;
                tt_VatTu.Visibility = Visibility.Collapsed;
                tt_KhachHang.Visibility = Visibility.Collapsed;
                tt_HoaDon.Visibility = Visibility.Collapsed;
                tt_PhieuThuTien.Visibility = Visibility.Collapsed;
                tt_BaoCao.Visibility = Visibility.Collapsed;
                tt_TraCuu.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_Home.Visibility = Visibility.Visible;
                tt_NhanVien.Visibility = Visibility.Visible;
                tt_LoaiVatTu.Visibility = Visibility.Visible;
                tt_VatTu.Visibility = Visibility.Visible;
                tt_KhachHang.Visibility = Visibility.Visible;
                tt_HoaDon.Visibility = Visibility.Visible;
                tt_PhieuThuTien.Visibility = Visibility.Visible;
                tt_BaoCao.Visibility = Visibility.Visible;
                tt_TraCuu.Visibility = Visibility.Visible;
            }
        }

        private void togglebutton_Unchecked(object sender, RoutedEventArgs e)
        {
            Pages.Opacity = 1;
        }

        private void togglebutton_Checked(object sender, RoutedEventArgs e)
        {
            Pages.Opacity = 0.3;
        }

        private void contentuser_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            togglebutton.IsChecked = false;
        }

        private void closebutton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}