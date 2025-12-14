using PageNavigation.Model;
using PageNavigation.Utilities;
using PageNavigation.View.BaoCaoDetail;
using PageNavigation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PageNavigation.View.TraCuuDetail
{
    public class TraCuuNavigation : ViewModelBase
    {
		private string _currentTag;

		public string CurrentTag
		{
			get { return _currentTag; }
			set 
			{ 
				_currentTag = value;
				OnPropertyChanged();
				SwitchViewByTag(value);
			}
		}
		private object? _currentView;

		public object? CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(); }
		}
        private DanhSachKhachHangVM _danhSachKhachHangVM;
        private VatTuVM _vatTuVM;
        private void SwitchViewByTag(string tag)
        {
            switch (tag)
            {
                case "KhachHang": CurrentView = _danhSachKhachHangVM; break;
                case "VatTu": CurrentView = _vatTuVM; break;
                default: break;
            }
        }
        public ICommand KhachHangCommand { get; set; }
        public ICommand VatTuCommand { get; set; }

        private void KhachHang(object obj)
        { CurrentTag = "KhachHang"; }
        private void VatTu(object obj)
        { CurrentTag = "VatTu"; }
        public TraCuuNavigation()
        {
            _danhSachKhachHangVM = new DanhSachKhachHangVM();
            _vatTuVM = new VatTuVM();

            KhachHangCommand = new RelayCommand(KhachHang);
            VatTuCommand = new RelayCommand(VatTu);

            CurrentTag = "KhachHang";
        }
    }
}
