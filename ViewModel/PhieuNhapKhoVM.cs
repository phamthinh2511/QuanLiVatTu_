using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageNavigation.Model;

namespace PageNavigation.ViewModel
{
    class PhieuNhapKhoVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;



        public DateOnly HienThiNhapDonHang
        {
            get { return _pageModel.OrderDate; }
            set { _pageModel.OrderDate = value; OnPropertyChanged(); }
        }

        public PhieuNhapKhoVM()
        {
            _pageModel = new PageModel();
            HienThiNhapDonHang = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
