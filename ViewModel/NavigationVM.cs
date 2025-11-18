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
        public ICommand KhachHangCommand { get; set; }
        public ICommand VatTuCommand { get; set; }
        public ICommand NguoiDungCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand PhieuNhapKhoCommand { get; set; }
        public ICommand PhieuXuatKhoCommand { get; set; }
        public ICommand VanChuyenCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void KhachHang(object obj) => CurrentView = new KhachHangVM();
        private void VatTu(object obj) => CurrentView = new VatTuVM();
        private void PhieuNhapKho(object obj) => CurrentView = new PhieuNhapKhoVM();
        private void PhieuXuatKho(object obj) => CurrentView = new PhieuXuatKhoVM();
        private void NguoiDung(object obj) => CurrentView = new NguoiDungHeThongVM();
        private void VanChuyen(object obj) => CurrentView = new QuaTrinhVanChuyenVM();
        private void Setting(object obj) => CurrentView = new SettingsVM();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            KhachHangCommand = new RelayCommand(KhachHang);
            VatTuCommand = new RelayCommand(VatTu);
            PhieuNhapKhoCommand = new RelayCommand(PhieuNhapKho);
            PhieuXuatKhoCommand = new RelayCommand(PhieuXuatKho);
            NguoiDungCommand = new RelayCommand(NguoiDung);
            SettingsCommand = new RelayCommand(Setting);
            VanChuyenCommand = new RelayCommand(VanChuyen);

            CurrentView = new HomeVM();
        }
    }
}
