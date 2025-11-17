using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageNavigation.Model;

namespace PageNavigation.ViewModel
{
    class PhieuXuatKhoVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public decimal Tien
        {
            get { return _pageModel.TransactionValue; }
            set { _pageModel.TransactionValue = value; OnPropertyChanged(); }
        }

        public PhieuXuatKhoVM()
        {
            _pageModel = new PageModel();
            Tien = 0;
        }
    }
}
