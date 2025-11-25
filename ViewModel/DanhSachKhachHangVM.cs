using PageNavigation.Model;
using PageNavigation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNavigation.ViewModel
{
    public class DanhSachKhachHangVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        private ObservableCollection<KhachHangVM> _listCustomers;
        public ObservableCollection<KhachHangVM> ListCustomers
        {
            get { return _listCustomers; }
            set { _listCustomers = value; OnPropertyChanged(); }
        }

        public void AddCustomer(KhachHangVM newCustomer)
        {
            ListCustomers.Add(newCustomer);
        }
        public DanhSachKhachHangVM()
        {
            _pageModel = new PageModel();
            ListCustomers = new ObservableCollection<KhachHangVM>();
            LoadData();
        }

        private void LoadData()
        {
            ListCustomers.Add(new KhachHangVM
            {
                CustomerID = 1,
                CustomerName = "Nguyễn Văn A",
                CustomerContact = "0901234567",
                CustomerAddress = "123 Đường Láng, Hà Nội",
                CustomerGender = "Nam",
                CustomerBirth = DateOnly.ParseExact("25/11/2006", "dd/MM/yyyy", null)
            });

            ListCustomers.Add(new KhachHangVM
            {
                CustomerID = 2,
                CustomerName = "Trần Thị B",
                CustomerContact = "0912345678",
                CustomerAddress = "456 Cầu Giấy, Hà Nội",
                CustomerGender = "Nữ",
                CustomerBirth = DateOnly.ParseExact("25/11/2006", "dd/MM/yyyy", null)
            });

            ListCustomers.Add(new KhachHangVM
            {
                CustomerID = 3,
                CustomerName = "Lê Văn C",
                CustomerContact = "0987654321",
                CustomerAddress = "789 Kim Mã, Hà Nội",
                CustomerGender = "Khác",
                CustomerBirth = DateOnly.ParseExact("25/11/2006", "dd/MM/yyyy", null)
            });
            ListCustomers.Add(new KhachHangVM
            {
                CustomerID = 4,
                CustomerName = "Trần Văn D",
                CustomerContact = "0987631231",
                CustomerAddress = "123 Trần Hưng Đạo, Quận 1, TP HCM",
                CustomerGender = "Nam",
                CustomerBirth = DateOnly.ParseExact("25/11/2006", "dd/MM/yyyy", null)
            });
        }
    }
}
