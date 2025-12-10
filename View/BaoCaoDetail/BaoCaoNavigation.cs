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
        private object? _currentChart;

        public object? CurrentChart
        {
            get { return _currentChart; }
            set { _currentChart = value; OnPropertyChanged(); }
        }

        private ChiTietVM _chiTietVM;
        private TongQuanVM _tongQuanVM;

        public ICommand ChiTietCommand { get; set; }
        public ICommand TongQuanCommand { get; set; }

        private void ChiTiet(object obj) => CurrentChart = _chiTietVM;
        private void TongQuan(object obj) => CurrentChart = _tongQuanVM;

        public BaoCaoNavigation()
        {
            _chiTietVM = new ChiTietVM();
            _tongQuanVM = new TongQuanVM();

            ChiTietCommand = new RelayCommand(ChiTiet);
            TongQuanCommand = new RelayCommand(TongQuan);

            CurrentChart = _tongQuanVM;
        }
    }
}
