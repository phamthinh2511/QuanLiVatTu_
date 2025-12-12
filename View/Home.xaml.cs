using LiveCharts;
using LiveCharts.Wpf;
using PageNavigation.View.BaoCaoDetail;
using PageNavigation.View.HomeUserControl;
using PageNavigation.ViewModel;
using PageNavigation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PageNavigation;

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
            var vatTuVM = new VatTuVM();
            var hoaDonVM = new HoaDonVM();
            Binding myBinding = new Binding("TongDoanhThu");
            DoanhThuButton.number.SetBinding(TextBlock.TextProperty, myBinding);
            DoanhThuButton.DataContext = hoaDonVM;
            lvSanPham.DataContext = vatTuVM;
            DoanhThuButton.title.Text = "Doanh Thu";
            DoanhThuButton.title.Foreground = Brushes.Violet;
            DoanhThuButton.picture.Source = new BitmapImage(new Uri("/Images/cashhome.png", UriKind.Relative));
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current?.MainWindow as MainWindow;
            foreach (ListViewItem item in mainWindow.listview.Items)
            {
                
                if (item.Tag.ToString() == "VatTu")
                {
                    mainWindow.listview.SelectedItem = item;
                    item.IsSelected = true;
                    item.Focus();
                    break;
                }
            }
        }
    }
}
