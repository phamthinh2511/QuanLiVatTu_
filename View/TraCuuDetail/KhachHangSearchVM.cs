using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using PageNavigation.Utilities;
using PageNavigation.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNavigation.View.TraCuuDetail
{
    public class KhachHangSearchVM : ViewModelBase
    {
        private string _khTen;

        public string KHTen
        {
            get { return _khTen; }
            set 
            {
                if (_khTen != value)
                {
                    _khTen = value;
                    OnPropertyChanged();
                    LoadKhachHangData();
                }
            }
        }
        private string _khSDT;

        public string KHSDT
        {
            get { return _khSDT; }
            set 
            {
                if (_khSDT != value)
                {
                    _khSDT = value;
                    OnPropertyChanged();
                    LoadKhachHangData();
                }
            }
        }
        private ObservableCollection<KhachHangM> _listKhachHang;

        public ObservableCollection<KhachHangM> ListKhachHang
        {
            get { return _listKhachHang; }
            set { _listKhachHang = value; OnPropertyChanged(); }
        }
        public KhachHangSearchVM()
        {
            ListKhachHang = new ObservableCollection<KhachHangM>();
        }
        private void LoadKhachHangData()
        {
            string tenkh = KHTen ?? "";
            string sdtkh = KHSDT ?? "";
            if (string.IsNullOrEmpty(tenkh) && string.IsNullOrEmpty(sdtkh))
            {
                ListKhachHang = new ObservableCollection<KhachHangM>();
                return;
            }

            using (var context = new QuanLyVatTuContext())
            {
                var query = context.KhachHang.AsQueryable();
                var data = query.Where(x =>
                    (string.IsNullOrEmpty(tenkh) || x.HoVaTen.Contains(tenkh)) &&
                    (string.IsNullOrEmpty(sdtkh) || x.SoDienThoai.Contains(sdtkh))
                ).ToList();

                ListKhachHang = new ObservableCollection<KhachHangM>(data);
            }
        }
    }
}
