using PageNavigation.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using static PageNavigation.Service.BaoCaoService;

namespace PageNavigation.View.BaoCaoDetail
{
        public class DoanhThuVM : ViewModelBase
        {
            private DoanhThuService _service;

            private ObservableCollection<int> _listNam;
            public ObservableCollection<int> ListNam
            {
                get => _listNam;
                set { _listNam = value; OnPropertyChanged(); }
            }

            private int _namBaoCao;
            public int NamBaoCao
            {
                get => _namBaoCao;
                set
                {
                    _namBaoCao = value;
                    OnPropertyChanged();

                    if (_namBaoCao >= 2000)
                    {
                        LoadDoanhThu();
                    }
                }
            }

            private ObservableCollection<BaoCaoDoanhThuDTO> _listDoanhThu;
            public ObservableCollection<BaoCaoDoanhThuDTO> ListDoanhThu
            {
                get => _listDoanhThu;
                set { _listDoanhThu = value; OnPropertyChanged(); }
            }

            public DoanhThuVM()
            {
                _service = new DoanhThuService();
                ListNam = new ObservableCollection<int>();

                int currentYear = DateTime.Now.Year;
                for (int i = 2000; i <= currentYear + 1; i++)
                {
                    ListNam.Add(i);
                }

                NamBaoCao = currentYear;
            }

            private async void LoadDoanhThu()
            {

                await Task.Run(() =>
                {

                    var data = _service.GetBaoCaoDoanhThuTheoNam(NamBaoCao);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ListDoanhThu = new ObservableCollection<BaoCaoDoanhThuDTO>(data);
                    });
                });
            }
        }
    }


