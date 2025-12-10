using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace PageNavigation.ViewModel
{
    
        public class LoaiVatTuVM : Utilities.ViewModelBase
        {
            public ObservableCollection<LoaiVatTuM> DanhSachLoaiVatTu { get; set; }
            private LoaiVatTuM _currentLoaiVatTu;
        public LoaiVatTuM CurrentLoaiVatTu
        {
            get { return _currentLoaiVatTu; }
            set 
            { 
                _currentLoaiVatTu = value; 
                OnPropertyChanged(); // Thông báo cho giao diện biết có thay đổi
            }
        }
            public LoaiVatTuVM()
            {
                LoadDataAsync();
            }

        public async Task LoadDataAsync()
        {
            using (var context = new QuanLyVatTuContext())
            {
                var data = await context.LoaiVatTu.ToListAsync();

                DanhSachLoaiVatTu = new ObservableCollection<LoaiVatTuM>(
                    data.Select(lvt => new LoaiVatTuM
                    {
                        MaLoai = lvt.MaLoai,
                        TenLoai = lvt.TenLoai,
                        MoTa = lvt.MoTa
                    })
                );

                OnPropertyChanged(nameof(DanhSachLoaiVatTu));
            }
        }
        public async Task<bool> IsTenLoaiExistsAsync(string tenLoai)
        {
            using (var context = new QuanLyVatTuContext())
            {
                return await context.LoaiVatTu
                    .AnyAsync(x => x.TenLoai.ToLower().Trim() == tenLoai.ToLower().Trim());
            }
        }

        public void LoadPage()
        {
            DanhSachLoaiVatTu = new ObservableCollection<LoaiVatTuM>
            {
                // Mẫu 1: Bình thường
                new LoaiVatTuM
                {
                    MaLoai = 1,
                    TenLoai = "Linh Kiện Điện Tử",
                    MoTa = "Bao gồm CPU, RAM, Mainboard và các linh kiện bán dẫn khác."
                },

                // Mẫu 2: Test chuỗi dài (Để xem khung xám có giãn dòng hay không)
                new LoaiVatTuM
                {
                    MaLoai = 2,
                    TenLoai = "Vật Liệu Xây Dựng",
                    MoTa = "Gồm xi măng, cát, đá, sỏi. Đây là dòng mô tả rất dài được viết ra nhằm mục đích kiểm tra xem TextBox có tự động xuống dòng (TextWrapping) khi hết chỗ hay không."
                },

                // Mẫu 3: Mô tả trống (null)
                new LoaiVatTuM
                {
                    MaLoai = 3,
                    TenLoai = "Văn Phòng Phẩm",
                    MoTa = null
                },

                // Mẫu 4: Test tiếng Việt
                new LoaiVatTuM
                {
                    MaLoai = 4,
                    TenLoai = "Thực Phẩm Đóng Hộp",
                    MoTa = "Các loại đồ hộp, mì gói, thực phẩm khô bảo quản lâu ngày."
                }
            };
        }
    }

    
}
