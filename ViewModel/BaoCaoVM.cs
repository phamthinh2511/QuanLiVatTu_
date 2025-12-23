using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using PageNavigation.Service;
using PageNavigation.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                    LoadData();
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
                    LoadData();
                }
            }
        }
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged();  }
        }


        private ObservableCollection<BaoCaoM> _listBaoCao;

		public ObservableCollection<BaoCaoM> ListBaoCao
		{
			get { return _listBaoCao; }
			set { _listBaoCao = value; OnPropertyChanged(); }
		}


        public ObservableCollection<int> ListNam { get; set; } = new ObservableCollection<int>();
        public BaoCaoVM()
        {
            ListBaoCao = new ObservableCollection<BaoCaoM>();
            ThangBaoCao = DateTime.Now.Month;
            NamBaoCao = DateTime.Now.Year;
            for (int i = 2000; i <= DateTime.Now.Year + 1; i++)
            {
                ListNam.Add(i);
            }
        }
        private async void LoadData()
        {
            IsBusy = true;

            await Task.Run(() =>
            {
                try
                {
                    BaoCaoService.TinhVaLuuBaoCaoTonKho(ThangBaoCao, NamBaoCao);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
                }
            });

            LoadBaoCaoData();

            IsBusy = false;
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
