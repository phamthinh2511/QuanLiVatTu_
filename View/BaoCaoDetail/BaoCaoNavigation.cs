using PageNavigation.Model;
using PageNavigation.Utilities;
using PageNavigation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PageNavigation.View.BaoCaoDetail
{
    public class BaoCaoNavigation : ViewModelBase
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
        private DoanhThuVM _doanhThuVM;
        private TonKhoVM _tonKhoVM;
        private void SwitchViewByTag(string tag)
        {
            switch (tag)
            {
                case "TonKho": CurrentView = _tonKhoVM; break;
                case "DoanhThu": CurrentView = _doanhThuVM; break;            
                default: break;
            }
        }
        public ICommand DoanhThuCommand { get; set; }
        public ICommand TonKhoCommand { get; set; }

        private void DoanhThu(object obj)
        { CurrentTag = "DoanhThu"; }
        private void TonKho(object obj)
        { CurrentTag = "TonKho"; }

        public BaoCaoNavigation()
        {
            _doanhThuVM = new DoanhThuVM();
            _tonKhoVM = new TonKhoVM();

            TonKhoCommand = new RelayCommand(TonKho);
            DoanhThuCommand = new RelayCommand(DoanhThu);

            CurrentTag = "TonKho";
        }
    }
}
