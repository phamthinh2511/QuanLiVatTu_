using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace PageNavigation.View.BaoCaoDetail.PopupChart
{
    public partial class TonKhoChart : Window, INotifyPropertyChanged
    {
        // Các biến Binding ra màn hình
        public SeriesCollection ChartSeries { get; set; }
        public string[] ProductLabels { get; set; }
        public double ChartWidth { get; set; }
        public double TotalQuantity { get; set; }
        public int TotalItems { get; set; }
        public string NamBaocao { get; set; }   

        // Constructor
        public TonKhoChart(IEnumerable<dynamic> inventoryData, int nam = 2025)
        {
            InitializeComponent();

            NamBaocao = nam.ToString(); 
            var dataList = inventoryData.ToList();
            int count = dataList.Count;
            ChartWidth = Math.Max(950, count * 60);
            ProductLabels = dataList.Select(x => (string)x.MaVatTuNavigation.TenVatTu).ToArray();
            var listTonCuoi = dataList.Select(x => (double)x.TonCuoi).ToList();
            var columnValues = new ChartValues<double>(listTonCuoi);

            ChartSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Tồn cuối",
                    Values = columnValues,
                    DataLabels = true,
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#10b981"),
                    MaxColumnWidth = 35
                }
            };
            TotalItems = count;
            TotalQuantity = listTonCuoi.Sum();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}