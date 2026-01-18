using LiveCharts;
using LiveCharts.Configurations;
using PageNavigation.View.HomeUserControl;
using PageNavigation.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            this.DataContext = new HomeVM();
            var vatTuVM = new VatTuVM();
            var hoaDonVM = new HoaDonVM();
            lvSanPham.DataContext = vatTuVM;
          
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
