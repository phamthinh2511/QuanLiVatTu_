using LiveCharts;
using LiveCharts.Configurations;
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
        public ChartValues<double> DoanhThuValues { get; set; }
        public ObservableCollection<string> Labels { get; set; }
        public object ColorMapper { get; set; }
        public Home()
        {
            InitializeComponent();
            var dangerBrush = new SolidColorBrush(Color.FromRgb(238, 83, 83));
            var blueBrush = new SolidColorBrush(Color.FromRgb(74, 144, 226)); 
            var yellowBrush = new SolidColorBrush(Color.FromRgb(255, 206, 84));
            var purpleBrush = new SolidColorBrush(Color.FromRgb(148, 0, 211)); 

            var colors = new List<Brush> { dangerBrush, blueBrush, yellowBrush, purpleBrush };
            ColorMapper = Mappers.Xy<double>()
                .X(value => value)
                .Y((value, index) => index)
                .Fill((value, index) => colors[index % colors.Count])
                .Stroke((value, index) => colors[index % colors.Count]);
            DoanhThuValues = new ChartValues<double> { 10, 50, 39, 50 };
            Labels = new ObservableCollection<string> { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4" };
            var vatTuVM = new VatTuVM();
            var hoaDonVM = new HoaDonVM();
            Binding myBinding = new Binding("TongDoanhThu");
            DoanhThuButton.number.SetBinding(TextBlock.TextProperty, myBinding);
            DoanhThuButton.DataContext = hoaDonVM;
            chart.DataContext = this;
            lvSanPham.DataContext = vatTuVM;
            DoanhThuButton.title.Text = "Doanh Thu";
            DoanhThuButton.title.Foreground = Brushes.Violet;
            DoanhThuButton.picture.Source = new BitmapImage(new Uri("/Images/cashhome.png", UriKind.Relative));
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
