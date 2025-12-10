using PageNavigation.Model;
using PageNavigation.Utilities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System;

namespace PageNavigation.ViewModel
{
    public class NhanVienVM : ViewModelBase
    {
        // 1. Khai báo danh sách Nhân viên (Binding vào ListView bên XAML)
        // Lưu ý: Class NhanVien phải khớp với tên class trong Model của bạn
        private ObservableCollection<NhanVienM> _listEmployees;
        public ObservableCollection<NhanVienM> ListEmployees
        {
            get { return _listEmployees; }
            set
            {
                _listEmployees = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(); }
        }

        // Constructor
        public NhanVienVM()
        {
            LoadDataAsync();
        }

        // 2. Hàm tải dữ liệu từ CSDL
        public async void LoadDataAsync()
        {
            try
            {
                IsLoading = true;

                using (var context = new QuanLyVatTuContext())
                {
                    // Giả định bảng trong CSDL tên là NhanVien
                    // Sắp xếp theo ID giảm dần để nhân viên mới nhất lên đầu
                    var data = await context.Nhanviens.OrderByDescending(x => x.MaNhanVien).ToListAsync();

                    ListEmployees = new ObservableCollection<NhanVienM>(data);
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

        // 3. Hàm Thêm Nhân Viên
        public void AddNhanVien(NhanVienM nv)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    context.Nhanviens.Add(nv);
                    context.SaveChanges();
                }
                // Tải lại dữ liệu sau khi thêm
                LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        // 4. Hàm Xóa Nhân Viên
        public void DeleteNhanVien(NhanVienM nv)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    // Tìm nhân viên trong DB dựa trên ID
                    var itemToDelete = context.Nhanviens.SingleOrDefault(x => x.MaNhanVien == nv.MaNhanVien);

                    if (itemToDelete != null)
                    {
                        context.Nhanviens.Remove(itemToDelete);
                        context.SaveChanges();
                    }
                }
                // Tải lại dữ liệu sau khi xóa
                LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
        }
    }
}