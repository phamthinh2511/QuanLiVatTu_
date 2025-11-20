using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNavigation.ViewModel
{
    class KhachHangVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        private int _customerid;
        private string _customername;
        private string _customercontact;
        private string _customeraddress;
        private string _customergender;

        public ObservableCollection<string> GenderOptions { get; set; }


        public int CustomerID
        {
            get { return _customerid; }
            set { _customerid = value; OnPropertyChanged(); }
        }

        public string CustomerName
        {
            get { return _customername; }
            set { _customername = value; OnPropertyChanged(); }
        }

        public string CustomerContact
        {
            get { return _customercontact; }
            set { _customercontact = value; OnPropertyChanged(); }
        }

        public string CustomerAddress
        {
            get { return _customeraddress; }
            set { _customeraddress = value; OnPropertyChanged(); }
        }

        public string CustomerGender
        {
            get { return _customergender; }
            set { _customergender = value; OnPropertyChanged(); }
        }

        public KhachHangVM()
        {
            _pageModel = new PageModel();
            CustomerID = 1;
            GenderOptions = new ObservableCollection<string>
            {
                "Nam",
                "Nữ",
                "Khác"
            };
        }
    }
}
