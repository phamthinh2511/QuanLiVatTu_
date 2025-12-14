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
    internal class VatTuSearchVM : ViewModelBase
    {
        private string _vtTen;

        public string VTTen
        {
            get { return _vtTen; }
            set 
            { 
                if (_vtTen != value)
                {
                    _vtTen = value;
                    OnPropertyChanged();
                    LoadVatTuData();
                }
                
            }
        }
        private string _vtLoai;

        public string VTLoai
        {
            get { return _vtLoai; }
            set 
            {
                if (_vtLoai != value)
                {
                    _vtLoai = value;
                    OnPropertyChanged();
                    LoadVatTuData();
                }
            }
        }
        private ObservableCollection<VatTuM> _listVatTu;

        public ObservableCollection<VatTuM> ListVatTu
        {
            get { return _listVatTu; }
            set { _listVatTu = value; OnPropertyChanged(); }
        }
        public VatTuSearchVM()
        {
            ListVatTu = new ObservableCollection<VatTuM>();

        }

        public void LoadVatTuData()
        {
            string tenvt = VTTen ?? "";
            string loaivt = VTLoai ?? "";
            if (string.IsNullOrEmpty(tenvt) && string.IsNullOrEmpty(loaivt))
            {
                ListVatTu = new ObservableCollection<VatTuM>();
                return;
            }

            using (var context = new QuanLyVatTuContext())
            {
                var query = context.VatTu.AsQueryable();
                query = query.Include(x => x.MaLoaiNavigation);

                if (!string.IsNullOrEmpty(tenvt))
                {
                    query = query.Where(x => x.TenVatTu.Contains(tenvt));
                }

                var data = query.ToList();
                ListVatTu = new ObservableCollection<VatTuM>(data);
            }
        }
    }
}
