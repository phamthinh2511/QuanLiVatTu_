using PageNavigation.Model;
using PageNavigation.Session; // Giả sử class UserSession nằm ở đây
using PageNavigation.Utilities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PageNavigation.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        public ObservableCollection<MenuItemModel> MenuItems { get; set; }

        private object? _currentView;
        public object? CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        private MenuItemModel _selectedMenuItem;
        public MenuItemModel SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                _selectedMenuItem = value;
                OnPropertyChanged();

                if (_selectedMenuItem != null)
                {
                    SwitchViewByTag(_selectedMenuItem.ViewTag);
                }
            }
        }
        public ICommand NavigateCommand { get; set; }
        private HomeVM _homeVM;
        private NhanVienVM _nhanVienVM;
        private LoaiVatTuVM _loaiVatTuVM;
        private DanhSachKhachHangVM _danhSachKhachHangVM;
        private VatTuVM _vatTuVM;
        private HoaDonVM _hoaDonVM;
        private PhieuNhapVatTuVM _phieuNhapVatTuVM;
        private PhieuThuTienVM _phieuThuTienVM;
        private TraCuuVM _traCuuVM;
        private BaoCaoVM _baoCaoVM;

        public NavigationVM()
        {
            MenuItems = new ObservableCollection<MenuItemModel>();
            _homeVM = new HomeVM();
            _nhanVienVM = new NhanVienVM();
            _loaiVatTuVM = new LoaiVatTuVM();
            _danhSachKhachHangVM = new DanhSachKhachHangVM();
            _vatTuVM = new VatTuVM();
            _hoaDonVM = new HoaDonVM();
            _phieuNhapVatTuVM = new PhieuNhapVatTuVM();
            _phieuThuTienVM = new PhieuThuTienVM();
            _traCuuVM = new TraCuuVM();
            _baoCaoVM = new BaoCaoVM();

            LoadMenuBasedOnRole();

            SelectedMenuItem = MenuItems.FirstOrDefault(x => x.ViewTag == "Home");
            NavigateCommand = new RelayCommand(NavigateToPage);
        }
        private void NavigateToPage(object parameter)
        {
            if (parameter is string tag)
            {
                var item = MenuItems.FirstOrDefault(x => x.ViewTag == tag);
                if (item != null)
                {
                    SelectedMenuItem = item;
                }
            }
        }
        private void LoadMenuBasedOnRole()
        {
            string role = UserSession.RoleName;
            MenuItems.Add(new MenuItemModel { Title = "Home", IconPath = "/Images/img_home.png", ViewTag = "Home" });
            if (role == "QuanLy")
            {
                MenuItems.Add(new MenuItemModel { Title = "Nhân Viên", IconPath = "/Images/icons8-staff-96.png", ViewTag = "NhanVien" });
                MenuItems.Add(new MenuItemModel { Title = "Loại Vật Tư", IconPath = "/Images/icons8-material-ui-48.png", ViewTag = "LoaiVatTu" });
                MenuItems.Add(new MenuItemModel { Title = "Vật Tư", IconPath = "/Images/img_product.png", ViewTag = "VatTu" });
                MenuItems.Add(new MenuItemModel { Title = "Khách Hàng", IconPath = "/Images/img_customer.png", ViewTag = "KhachHang" });
                MenuItems.Add(new MenuItemModel { Title = "Hóa Đơn", IconPath = "/Images/img_order.png", ViewTag = "HoaDon" });
                MenuItems.Add(new MenuItemModel { Title = "Phiếu Nhập", IconPath = "/Images/icons8-to-do-list-96.png", ViewTag = "PhieuNhapVatTu" });
                MenuItems.Add(new MenuItemModel { Title = "Tra Cứu", IconPath = "/Images/icons8-search-100 (1).png", ViewTag = "TraCuu" });
                MenuItems.Add(new MenuItemModel { Title = "Báo Cáo", IconPath = "/Images/icons8-report-100.png", ViewTag = "BaoCao" });
            }
            else if (role == "ThuKho")
            {
                MenuItems.Add(new MenuItemModel { Title = "Loại Vật Tư", IconPath = "/Images/icons8-material-ui-48.png", ViewTag = "LoaiVatTu" });
                MenuItems.Add(new MenuItemModel { Title = "Vật Tư", IconPath = "/Images/img_product.png", ViewTag = "VatTu" });
                MenuItems.Add(new MenuItemModel { Title = "Phiếu Nhập", IconPath = "/Images/icons8-to-do-list-96.png", ViewTag = "PhieuNhapVatTu" });
            }
            else if (role == "NVBanHang")
            {
                MenuItems.Add(new MenuItemModel { Title = "Khách Hàng", IconPath = "/Images/img_customer.png", ViewTag = "KhachHang" });
                MenuItems.Add(new MenuItemModel { Title = "Hóa Đơn", IconPath = "/Images/img_order.png", ViewTag = "HoaDon" });
                MenuItems.Add(new MenuItemModel { Title = "Tra Cứu", IconPath = "/Images/icons8-search-100 (1).png", ViewTag = "TraCuu" });
                MenuItems.Add(new MenuItemModel { Title = "Báo Cáo", IconPath = "/Images/icons8-report-100.png", ViewTag = "BaoCao" });
            }
        }

        private void SwitchViewByTag(string tag)
        {
            switch (tag)
            {
                case "Home": CurrentView = _homeVM; break;
                case "NhanVien": CurrentView = _nhanVienVM; break;
                case "LoaiVatTu": CurrentView = _loaiVatTuVM; break;
                case "KhachHang": CurrentView = _danhSachKhachHangVM; break;
                case "VatTu": CurrentView = _vatTuVM; break;
                case "HoaDon": CurrentView = _hoaDonVM; break;
                case "PhieuNhapVatTu": CurrentView = _phieuNhapVatTuVM; break;
                case "PhieuThuTien": CurrentView = _phieuThuTienVM; break;
                case "TraCuu": CurrentView = _traCuuVM; break;
                case "BaoCao": CurrentView = _baoCaoVM; break;
                default: break;
            }
        }
    }
}