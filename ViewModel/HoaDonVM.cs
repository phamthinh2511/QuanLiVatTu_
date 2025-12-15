using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PageNavigation.ViewModel 
{
    public class HoaDonVM : Utilities.ViewModelBase
    {

        private string _tongDoanhThu;
        public string TongDoanhThu
        {
            get { return _tongDoanhThu; }
            set { _tongDoanhThu = value; OnPropertyChanged(); }
        }


        private ObservableCollection<HoaDonM> _billList;
        private ObservableCollection<HoaDonM> _listHoaDon;
        public ObservableCollection<HoaDonM> ListHoaDon
        {
            get { return _listHoaDon; }
            set { _listHoaDon = value; OnPropertyChanged(); }
        }


        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(); }
        }

        // Constructor
        public HoaDonVM()
        {

            LoadData();
        }


        public async void LoadData()
        {
            try
            {
                IsLoading = true;
                using (var context = new QuanLyVatTuContext())
                {

                    var data = await context.HoaDon
                                            .Include(h => h.MaNhanVienNavigation)
                                            .Include(h => h.MaKhachHangNavigation)
                                            .OrderByDescending(x => x.NgayLapHoaDon)
                                            .ToListAsync();

                    foreach (var item in data)
                    {
                        item.TenNhanVien = item.MaNhanVienNavigation?.HoTen ?? "---";
                        item.TenKhachHang = item.MaKhachHangNavigation?.HoVaTen ?? "---";
                    }


                    ListHoaDon = new ObservableCollection<HoaDonM>(data);


                    decimal total = data.Sum(x => x.TongTien ?? 0);
                    if (total >= 1000000000)
                        TongDoanhThu = $"{total / 1000000000:0.0} (tỷ)";
                    else
                        TongDoanhThu = $"{total:N0} đ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

    }
}

