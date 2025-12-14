using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using PageNavigation.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PageNavigation.ViewModel
{
    public class BaoCaoVM : ViewModelBase
    {
		private int _thangBaoCao;

		public int ThangBaoCao
		{
			get { return _thangBaoCao; }
			set 
            {
                if (_thangBaoCao != value)
                {
                    _thangBaoCao = value;
                    OnPropertyChanged();
                    LoadBaoCaoData();
                }
            }
		}
        private int _namBaoCao;

        public int NamBaoCao
        {
            get { return _namBaoCao; }
            set 
            {
                if (_namBaoCao != value)
                {
                    _namBaoCao = value;
                    OnPropertyChanged();
                    LoadBaoCaoData();
                }
            }
        }


        private ObservableCollection<BaoCaoM> _listBaoCao;

		public ObservableCollection<BaoCaoM> ListBaoCao
		{
			get { return _listBaoCao; }
			set { _listBaoCao = value; OnPropertyChanged(); }
		}



        public BaoCaoVM()
        {
            ListBaoCao = new ObservableCollection<BaoCaoM>();
            ThangBaoCao = DateTime.Now.Month;
            NamBaoCao = DateTime.Now.Year;
        }
        private void LoadBaoCaoData()
        {

            int thang = ThangBaoCao;
            int nam = NamBaoCao;

            using (var context = new QuanLyVatTuContext())
            {
                var data = context.BaoCao
                                  .Include(x => x.MaVatTuNavigation)
                                  .Where(x => x.Thang == thang && x.Nam == nam)
                                  .ToList();

                ListBaoCao = new ObservableCollection<BaoCaoM>(data);
            }
        }
    }
}
