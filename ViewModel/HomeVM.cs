using LiveCharts;
using LiveCharts.Wpf;
using PageNavigation.Model;
using PageNavigation.Utilities;
using PageNavigation.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PageNavigation.ViewModel
{
    public class HomeVM : ViewModelBase
    {
        public VatTuVM SanPhamVM { get; set; }
        public DoanhThuSoSanhVM ChartVM { get; set; }
        public HoaDonVM HoaDonVM { get; set; }

        private decimal _doanhThuThangNay;
        public decimal DoanhThuThangNay
        {
            get { return _doanhThuThangNay; }
            set { _doanhThuThangNay = value; OnPropertyChanged(); }
        }
        private void LoadDoanhThu()
        {

            var today = DateTime.Now;

            using (var context = new QuanLyVatTuContext())
            {
                var doanhThu = context.HoaDon
                    .Where(x => x.NgayLapHoaDon.Value.Month == today.Month && x.NgayLapHoaDon.Value.Year == today.Year)
                    .Sum(x => (decimal?)x.TongTien) ?? 0;

                DoanhThuThangNay = doanhThu;
            }
        }
        public HomeVM()
        {
            SanPhamVM = new VatTuVM();
            ChartVM = new DoanhThuSoSanhVM();
            HoaDonVM = new HoaDonVM();
            LoadDoanhThu();
        }
    }
    public class DoanhThuSoSanhVM : ViewModelBase 
    {
        public SeriesCollection ChartSeries { get; set; }
        public string[] ChartLabels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private DateTime _startDate = DateTime.Now.AddDays(-30);
        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); LoadData(); }
        }

        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); LoadData(); }
        }

        private decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set { _totalRevenue = value; OnPropertyChanged(); }
        }

        private double _percentChange;
        public double PercentChange
        {
            get => _percentChange;
            set { _percentChange = value; OnPropertyChanged(); }
        }

        private bool _isGrowth;
        public bool IsGrowth
        {
            get => _isGrowth;
            set { _isGrowth = value; OnPropertyChanged(); }
        }

        public DoanhThuSoSanhVM()
        {
            YFormatter = value => value.ToString("N0");
            LoadData();
        }
        private void LoadData()
        {
            DateTime startLastYear = StartDate.AddYears(-1);
            DateTime endLastYear = EndDate.AddYears(-1);

            List<HoaDonM> listNamNay = new List<HoaDonM>();
            List<HoaDonM> listNamTruoc = new List<HoaDonM>();
            using (var context = new QuanLyVatTuContext())
            {
                listNamNay = context.HoaDon
                    .Where(x => x.NgayLapHoaDon.Value >= StartDate &&
                                x.NgayLapHoaDon.Value <= EndDate &&
                                x.NgayLapHoaDon.HasValue)
                    .ToList();
                listNamTruoc = context.HoaDon
                    .Where(x => x.NgayLapHoaDon.Value >= startLastYear &&
                                x.NgayLapHoaDon.Value <= endLastYear &&
                                x.NgayLapHoaDon.HasValue)
                    .ToList();
            }

            ProcessChartData(listNamNay, listNamTruoc);
        }
        private void ProcessChartData(List<HoaDonM> listNamNay, List<HoaDonM> listNamTruoc)
        {
            var valuesNamNay = new ChartValues<double>();
            var valuesNamTruoc = new ChartValues<double>();
            var labels = new List<string>();

            double tongNamNay = 0;
            double tongNamTruoc = 0;
            int totalDays = (EndDate - StartDate).Days + 1;
            DateTime startLastYear = StartDate.AddYears(-1);

            for (int i = 0; i < totalDays; i++)
            {

                DateTime currentDay = StartDate.AddDays(i);
                DateTime correspondingDayLastYear = startLastYear.AddDays(i);
                double revenueToday = listNamNay
                    .Where(x => x.NgayLapHoaDon.Value.Date == currentDay.Date)
                    .Sum(x => (double)(x.TongTien ?? 0));

                double revenueLastYear = listNamTruoc
                    .Where(x => x.NgayLapHoaDon.Value.Date == correspondingDayLastYear.Date)
                    .Sum(x => (double)(x.TongTien ?? 0));

                valuesNamNay.Add(revenueToday);
                valuesNamTruoc.Add(revenueLastYear);
                labels.Add(currentDay.ToString("dd/MM"));
                tongNamNay += revenueToday;
                tongNamTruoc += revenueLastYear;
            }
            TotalRevenue = (decimal)tongNamNay;

            if (tongNamTruoc > 0)
            {
                PercentChange = ((tongNamNay - tongNamTruoc) / tongNamTruoc) * 100;
            }
            else if (tongNamNay > 0)
            {
                PercentChange = 100;
            }
            else
            {
                PercentChange = 0;
            }

            IsGrowth = PercentChange >= 0;
            ChartLabels = labels.ToArray();
            ChartSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Năm nay",
                    Values = valuesNamNay,
                    PointGeometry = null,
                    LineSmoothness = 0,
                    Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#2196F3"), 
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#332196F3"), 
                    StrokeThickness = 2
                },
                new LineSeries
                {
                    Title = "Năm trước",
                    Values = valuesNamTruoc,
                    PointGeometry = null,
                    LineSmoothness = 0,
                    Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF5722"),
                    Fill = Brushes.Transparent, 
                    StrokeThickness = 2
                }
            };
            OnPropertyChanged(nameof(ChartSeries));
            OnPropertyChanged(nameof(ChartLabels));
        }
    }
}
