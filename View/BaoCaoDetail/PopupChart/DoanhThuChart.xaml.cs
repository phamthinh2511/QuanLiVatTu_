using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace PageNavigation.View.BaoCaoDetail.PopupChart
{
    public partial class DoanhThuChart : Window, INotifyPropertyChanged
    {
        public SeriesCollection ChartSeries { get; set; }
        public string TotalRevenue { get; set; }
        public int TotalMonths { get; set; }
        public int NamBaocao { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }

        public DoanhThuChart(int nam, IEnumerable<dynamic> dataInput)
        {
            InitializeComponent();

            NamBaocao = nam;

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P0})", chartPoint.SeriesView.Title, chartPoint.Participation);

            ChartSeries = new SeriesCollection();

            var colors = new List<string> {
                                        "#3b82f6", "#8b5cf6", "#ec4899", "#f59e0b", "#10b981", "#6366f1",
                                        "#ef4444", "#f97316", "#84cc16", "#06b6d4", "#d946ef", "#e11d48"
            };

            int colorIndex = 0;
            double total = 0;
            int count = 0;

            foreach (var item in dataInput)
            {
                double doanhThu = Convert.ToDouble(item.TongDoanhThu);

                if (doanhThu <= 0) continue;
                string tenThang = "Tháng " + item.Thang;

                ChartSeries.Add(new PieSeries
                {
                    Title = tenThang,
                    Values = new ChartValues<double> { doanhThu },
                    DataLabels = true,
                    LabelPoint = PointLabel,
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(colors[colorIndex % colors.Count]),
                    PushOut = 5
                });
                total += doanhThu;
                count++;
                colorIndex++;
            }

            TotalRevenue = total.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
            TotalMonths = count;
            DataContext = this;
        }

        public class SalesData
        {
            public string Month { get; set; }
            public double Value { get; set; }
            public string Color { get; set; }
            public SalesData(string m, double v, string c) { Month = m; Value = v; Color = c; }
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