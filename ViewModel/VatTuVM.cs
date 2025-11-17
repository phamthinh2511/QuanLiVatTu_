using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageNavigation.Model;

namespace PageNavigation.ViewModel
{
    class VatTuVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public string TinhTrangVatTu
        {
            get { return _pageModel.ProductStatus; }
            set { _pageModel.ProductStatus = value; OnPropertyChanged(); }
        }

        public VatTuVM()
        {
            _pageModel = new PageModel();
            TinhTrangVatTu = "Hết Hàng";
        }
    }
}
