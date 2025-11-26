using PageNavigation.Model;
using PageNavigation.Utilities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System;

namespace PageNavigation.ViewModel
{
    public class DanhSachKhachHangVM : ViewModelBase
    {

        private ObservableCollection<KhachHangM> _listCustomers;
        public ObservableCollection<KhachHangM> ListCustomers
        {
            get { return _listCustomers; }
            set
            {
                _listCustomers = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public async void LoadDataAsync()
        {
            try
            {
                IsLoading = true;

                using (var context = new QuanLyVatTuContext())
                {
                    var data = await context.Khachhangs.OrderByDescending(x => x.MaKhachHang).ToListAsync();
                    ListCustomers = new ObservableCollection<KhachHangM>(data);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            finally
            {
                IsLoading = false;
            }
        }

        public DanhSachKhachHangVM()
        {
            LoadDataAsync();
        }


        public void AddCustomer(KhachHangM kh)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    context.Khachhangs.Add(kh);
                    context.SaveChanges();
                }
                LoadDataAsync();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi thêm: " + ex.Message); }
        }

        public void DeleteCustomer(KhachHangM kh)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    var itemToDelete = context.Khachhangs.SingleOrDefault(x => x.MaKhachHang == kh.MaKhachHang);
                    if (itemToDelete != null)
                    {
                        context.Khachhangs.Remove(itemToDelete);
                        context.SaveChanges();
                    }
                }
                LoadDataAsync(); // Gọi lại hàm này để cập nhật danh sách
            }
            catch (Exception ex) { MessageBox.Show("Lỗi xóa: " + ex.Message); }
        }
    }
}