using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PageNavigation.Utilities;
namespace PageNavigation.ViewModel
{
    public class HomeVM : ViewModelBase
    {
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
            LoadDoanhThu();
        }
    }
}
