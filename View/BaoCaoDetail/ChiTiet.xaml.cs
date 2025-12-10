using LiveCharts;
using LiveCharts.Wpf;
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

namespace PageNavigation.View.BaoCaoDetail
{
    /// <summary>
    /// Interaction logic for ChiTiet.xaml
    /// </summary>
    public partial class ChiTiet : UserControl
    {
        public ChiTiet()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "val1",
                    Values = new ChartValues<double> {5, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20 }
                }
            };
            SeriesCollection1 = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "val1",
                    Values = new ChartValues<double> {5}
                }
            };
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "val2",
                Values = new ChartValues<double> { 10, 15, 20, 25, 20, 20, 20, 20, 20, 20, 20, 20 }
            });
            BarLabels = new[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10"
                                , "Tháng 11", "Tháng 12"};
            Formatter = value => value.ToString("N");
            DataContext = this;
        }
        public SeriesCollection SeriesCollection1 { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] BarLabels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
