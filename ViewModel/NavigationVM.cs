using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageNavigation.Utilities;
using System.Windows.Input;

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

        private HomeVM _homeVM;
        private NhanVienVM _nhanVienVM;
        private LoaiVatTuVM _loaiVatTuVM;
        private DanhSachKhachHangVM _danhSachKhachHangVM;
        private VatTuVM _vatTuVM;
        private HoaDonVM _hoaDonVM;
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
        public ICommand PhieuThuTienCommand { get; set; }
        public ICommand BaoCaoCommand { get; set; }

        private void Home(object obj) => CurrentView = _homeVM;
        private void NhanVien(object obj) => CurrentView = _nhanVienVM;
        private void LoaiVatTu(object obj) => CurrentView = _loaiVatTuVM;

        private void KhachHang(object obj) => CurrentView = _danhSachKhachHangVM;

        private void VatTu(object obj) => CurrentView = _vatTuVM;
        private void HoaDon(object obj) => CurrentView = _hoaDonVM;
        private void PhieuThuTien(object obj) => CurrentView = _phieuThuTienVM;
        private void TraCuu(object obj) => CurrentView = _traCuuVM;
        private void BaoCao(object obj) => CurrentView = _baoCaoVM;

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            NhanVienCommand = new RelayCommand(NhanVien);
            LoaiVatTuCommand = new RelayCommand(LoaiVatTu);
            KhachHangCommand = new RelayCommand(KhachHang);
            VatTuCommand = new RelayCommand(VatTu);
            HoaDonCommand = new RelayCommand(HoaDon);
            PhieuThuTienCommand = new RelayCommand(PhieuThuTien);
            BaoCaoCommand = new RelayCommand(BaoCao);
            TraCuuCommand = new RelayCommand(TraCuu);


            _homeVM = new HomeVM();
            _nhanVienVM = new NhanVienVM();
            _loaiVatTuVM = new LoaiVatTuVM();
            _danhSachKhachHangVM = new DanhSachKhachHangVM();
            _vatTuVM = new VatTuVM();
            _hoaDonVM = new HoaDonVM();
            _phieuThuTienVM = new PhieuThuTienVM();
            _traCuuVM = new TraCuuVM();
            _baoCaoVM = new BaoCaoVM();

            CurrentView = _homeVM;
        }
    }
}