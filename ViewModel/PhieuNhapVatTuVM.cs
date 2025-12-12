using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using PageNavigation.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PageNavigation.ViewModel
{
    public class PhieuNhapVatTuVM : ViewModelBase
    {
        private ObservableCollection<PhieuNhapVatTuM> _listPhieuNhap;
        public ObservableCollection<PhieuNhapVatTuM> ListPhieuNhap
        {
            get => _listPhieuNhap;
            set
            {
                _listPhieuNhap = value;
                OnPropertyChanged();
            }
        }

        public PhieuNhapVatTuVM()
        {
            ListPhieuNhap = new ObservableCollection<PhieuNhapVatTuM>();

            // 1. Gọi hàm tải dữ liệu ban đầu
            // Vì gọi trong Constructor không dùng được 'await', ta dùng '_ =' để bỏ qua cảnh báo
            _ = LoadDataAsync();

            // 2. Đăng ký sự kiện Reload
            // Sửa lỗi gọi sai tên hàm LoadData -> LoadDataAsync
            // Thêm 'async' và 'await' để gọi hàm bất đồng bộ đúng cách
            PageNavigation.Service.PhieuNhapVatTuService.PhieuNhapVatTuChanged += async (s, e) =>
            {
                await LoadDataAsync();
            };
        }

        public async Task LoadDataAsync()
        {
            using (var db = new QuanLyVatTuContext())
            {
                var data = await db.PhieuNhapVatTu
                                   .OrderByDescending(p => p.MaPhieuNhap)
                                   .ToListAsync();

                ListPhieuNhap = new ObservableCollection<PhieuNhapVatTuM>(data);
            }
        }
    }
}
