using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PageNavigation.ViewModel
{
    public partial class NhanVienVM : Utilities.ViewModelBase
    {
        public ObservableCollection<NhanVienM> DanhSachNhanVien { get; set; }

        public async Task LoadDataAsync()
        {
            using (var context = new QuanLyVatTuContext())
            {
                var data = await context.NhanVien.ToListAsync();

                DanhSachNhanVien = new ObservableCollection<NhanVienM>(
                    data.Select(nv => new NhanVienM
                    {
                        MaNhanVien = nv.MaNhanVien,
                        HoTen = nv.HoTen,
                        NgaySinh = nv.NgaySinh,
                        SoDienThoai = nv.SoDienThoai,
                        ChucVu = nv.ChucVu,
                        Username = nv.Username,
                        Password = nv.Password,
                        NgayNhanViec = nv.NgayNhanViec
                    })
                );

                OnPropertyChanged(nameof(DanhSachNhanVien));
            }
        }
        public NhanVienVM()
        {
            LoadDataAsync();
        }
        public void LoadPage()
        {
            DanhSachNhanVien = new ObservableCollection<NhanVienM>
            {
                new NhanVienM
                {
                    MaNhanVien = 1,
                    HoTen = "Nguyễn Văn A",
                    NgaySinh = new DateOnly(1995, 5, 12),
                    SoDienThoai = "0987654321",
                    ChucVu = "Quản lý",
                    Username = "nguyenvana",
                    Password = "123456",
                    NgayNhanViec = new DateOnly(2020, 1, 15)
                },
                new NhanVienM
                {
                    MaNhanVien = 2,
                    HoTen = "Trần Thị B",
                    NgaySinh = new DateOnly(1998, 11, 2),
                    SoDienThoai = "0912345678",
                    ChucVu = "Nhân viên",
                    Username = "tranthib",
                    Password = "abcdef",
                    NgayNhanViec = new DateOnly(2021, 6, 1)
                },
                new NhanVienM
                {
                    MaNhanVien = 3,
                    HoTen = "Lê Văn C",
                    NgaySinh = new DateOnly(2000, 2, 20),
                    SoDienThoai = "0901234567",
                    ChucVu = "Thu ngân",
                    Username = "levanc",
                    Password = "pass123",
                    NgayNhanViec = new DateOnly(2022, 3, 10)
                }
            };
        }
        public async void DeleteEmployee(NhanVienM nv)
        {
            try
            {
                // 1. Hiện hộp thoại xác nhận
                var result = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên {nv.HoTen}?",
                                             "Xác nhận xóa",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.No) return;

                // 2. Thực hiện xóa trong Database
                using (var context = new QuanLyVatTuContext())
                {
                    // Tìm nhân viên trong DB dựa vào Mã
                    var itemToDelete = await context.NhanVien
                                                    .SingleOrDefaultAsync(x => x.MaNhanVien == nv.MaNhanVien);

                    if (itemToDelete != null)
                    {
                        context.NhanVien.Remove(itemToDelete);
                        await context.SaveChangesAsync();
                    }
                }

                // 3. Cập nhật lại danh sách hiển thị trên giao diện
                // (Đảm bảo tên biến ListNhanVien trùng với tên list trong VM của bạn)
                DanhSachNhanVien.Remove(nv);

                MessageBox.Show("Xóa thành công!");
            }
            catch (Exception ex)
            {
                // 4. Xử lý lỗi ràng buộc dữ liệu (Foreign Key)
                // Lỗi này xảy ra nếu Nhân viên đã từng lập Hóa đơn, Phiếu nhập, v.v.
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE statement conflicted"))
                {
                    MessageBox.Show("Không thể xóa nhân viên này vì họ đã lập Hóa đơn hoặc có dữ liệu liên quan trong hệ thống!",
                                    "Cảnh báo ràng buộc dữ liệu",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
            }
        }





    }
}
