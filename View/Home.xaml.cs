using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PageNavigation.View.BaoCaoDetail;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PageNavigation.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
            DoanhThuButton.title.Text = "Doanh Thu";
            DoanhThuButton.number.Text = "10.3 ( tỷ )";
            DoanhThuButton.title.Foreground = Brushes.DarkViolet;
            DoanhThuButton.picture.Source = new BitmapImage(new Uri("/Images/cashhome.png", UriKind.Relative));
            DonHangButton.title.Text = "Đơn Hàng Đa Xử Lí";
            DonHangButton.number.Text = "99";
            DonHangButton.title.Foreground = Brushes.DarkGreen;
            DonHangButton.picture.Source = new BitmapImage(new Uri("/Images/tickhome.png", UriKind.Relative));
            DonHangChuaXuLiButton.title.Text = "Đơn Hàng Cần Xử Lí";
            DonHangChuaXuLiButton.number.Text = "74";
            DonHangChuaXuLiButton.title.Foreground = Brushes.DarkRed;
            DonHangChuaXuLiButton.picture.Source = new BitmapImage(new Uri("/Images/packagehome.png", UriKind.Relative));

        }

        private void DoanhThuButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DonHangButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DonHangChuaXuLiButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
