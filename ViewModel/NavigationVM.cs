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

        public ICommand HomeCommand { get; set; }
        public ICommand NhanVienCommand { get; set; }
        public ICommand LoaiVatTuCommand { get; set; }
        public ICommand KhachHangCommand { get; set; }
        public ICommand VatTuCommand { get; set; }
        public ICommand TraCuuCommand { get; set; }
        public ICommand HoaDonCommand { get; set; }
        public ICommand PhieuThuTienCommand { get; set; }
        public ICommand BaoCaoCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void NhanVien(object obj) => CurrentView = new NhanVienVM();
        private void LoaiVatTu(object obj) => CurrentView = new LoaiVatTuVM();
        private void KhachHang(object obj) => CurrentView = new DanhSachKhachHangVM();
        private void VatTu(object obj) => CurrentView = new VatTuVM();
        private void HoaDon(object obj) => CurrentView = new HoaDonVM();
        private void PhieuThuTien(object obj) => CurrentView = new PhieuThuTienVM();
        private void TraCuu(object obj) => CurrentView = new TraCuuVM();
        private void BaoCao(object obj) => CurrentView = new BaoCaoVM();

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

            CurrentView = new HomeVM();
        }
    }
}
