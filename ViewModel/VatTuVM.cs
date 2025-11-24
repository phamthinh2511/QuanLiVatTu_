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

        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; OnPropertyChanged(); }
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
            set { _description = value; OnPropertyChanged(); }
        }

        private string _countingUnitID;

        public string CountingUnitID
        {
            get { return _countingUnitID; }
            set { _countingUnitID = value; OnPropertyChanged(); }
        }

        private string _origin;

        public string Origin
        {
            get { return _origin; }
            set { _origin = value; OnPropertyChanged(); }
        }

        private string _providerID;

        public string ProviderID
        {
            get { return _providerID; }
            set { _providerID = value; OnPropertyChanged(); }
        }
        private int _productStock;

        public int ProductStock
        {
            get { return _productStock; }
            set { _productStock = value; OnPropertyChanged(); }
        }


        public VatTuVM()
        {
            _pageModel = new PageModel();
        }
    }
}
