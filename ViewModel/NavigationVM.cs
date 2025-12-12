using PageNavigation.Utilities;
using PageNavigation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PageNavigation.Model;

namespace PageNavigation.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object? _currentView;

        public object? CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        private string _currentTag;

        public string CurrentTag
        {
            get { return _currentTag; }
            set { _currentTag = value; OnPropertyChanged(); SwitchViewByTag(value); }
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
                case "PhieuThuTien": CurrentView = _phieuThuTienVM; break;
                case "TraCuu": CurrentView = _traCuuVM; break;
                case "BaoCao": CurrentView = _baoCaoVM; break;
                default: break;
            }
        }
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

        public ICommand HomeCommand { get; set; }
        public ICommand NhanVienCommand { get; set; }
        public ICommand LoaiVatTuCommand { get; set; }
        public ICommand KhachHangCommand { get; set; }
        public ICommand VatTuCommand { get; set; }
        public ICommand TraCuuCommand { get; set; }
        public ICommand HoaDonCommand { get; set; }
        public ICommand PhieuNhapVatTuCommand { get; set; }
        public ICommand PhieuThuTienCommand { get; set; }
        public ICommand BaoCaoCommand { get; set; }

        private void Home(object obj)
        { CurrentView = _homeVM; CurrentTag = "Home"; }
        private void NhanVien(object obj)
        { CurrentView = _nhanVienVM; CurrentTag = "NhanVien"; }
        private void LoaiVatTu(object obj)
        { CurrentView = _loaiVatTuVM; CurrentTag = "LoaiVatTu"; }
        private void KhachHang(object obj)
        { CurrentView = _danhSachKhachHangVM; CurrentTag = "KhachHang"; }

        private void VatTu(object obj)
        { CurrentView = _vatTuVM; CurrentTag = "VatTu"; }
        private void HoaDon(object obj)
        { CurrentView = _hoaDonVM; CurrentTag = "HoaDon"; }
        private void PhieuThuTien(object obj)
        { CurrentView = _phieuThuTienVM; CurrentTag = "PhieuThuTien"; }
        private void TraCuu(object obj)
        { CurrentView = _traCuuVM; CurrentTag = "TraCuu"; }
        private void BaoCao(object obj)
        { CurrentView = _baoCaoVM; CurrentTag = "BaoCao"; }

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            NhanVienCommand = new RelayCommand(NhanVien);
            LoaiVatTuCommand = new RelayCommand(LoaiVatTu);
            KhachHangCommand = new RelayCommand(KhachHang);
            VatTuCommand = new RelayCommand(VatTu);
            HoaDonCommand = new RelayCommand(HoaDon);
            PhieuNhapVatTuCommand = new RelayCommand(PhieuNhapVatTu);
            PhieuThuTienCommand = new RelayCommand(PhieuThuTien);
            BaoCaoCommand = new RelayCommand(BaoCao);
            TraCuuCommand = new RelayCommand(TraCuu);


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

            CurrentTag = "Home";
        }

    }
}