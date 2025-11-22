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

        private int _productID;

        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; OnPropertyChanged(); }
        }

        private string _productStatus;
        public string ProductStatus
        {
            get { return _productStatus; }
            set { _productStatus = value; OnPropertyChanged(); }
        }

        private int _productTypeID;

        public int ProductTypeID
        {
            get { return _productTypeID; }
            set { _productTypeID = value; OnPropertyChanged(); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _unit;

        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        private string _origin;

        public string Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        private string _providerID;

        public string ProviderID
        {
            get { return _providerID; }
            set { _providerID = value; }
        }


        public VatTuVM()
        {
            _pageModel = new PageModel();
            _productStatus = "Hết Hàng";
        }
    }
}
