using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageNavigation.Model;

namespace PageNavigation.ViewModel
{
    class QuaTrinhVanChuyenVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public TimeOnly ThoiGianVanChuyen
        {
            get { return _pageModel.Ship_Delivery; }
            set { _pageModel.Ship_Delivery = value; OnPropertyChanged(); }
        }

        public QuaTrinhVanChuyenVM()
        {
            _pageModel = new PageModel();
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
            ThoiGianVanChuyen = time;
        }
    }
}
