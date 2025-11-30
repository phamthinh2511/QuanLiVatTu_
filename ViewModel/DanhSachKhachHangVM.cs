using PageNavigation.Model;
using PageNavigation.Utilities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace PageNavigation.ViewModel
{
    public class DanhSachKhachHangVM : ViewModelBase
    {
        private ObservableCollection<KhachHangM> _listCustomers;
        public ObservableCollection<KhachHangM> ListCustomers
        {
            get { return _listCustomers; }
            set { _listCustomers = value; OnPropertyChanged(); }
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
                    var data = await context.KhachHang.OrderByDescending(x => x.MaKhachHang).ToListAsync();
                    ListCustomers = new ObservableCollection<KhachHangM>(data);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
            finally { IsLoading = false; }
        }

        public DanhSachKhachHangVM()
        {
            LoadDataAsync();
        }

        public async void AddCustomer(KhachHangM kh)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    context.KhachHang.Add(kh);
                    await context.SaveChangesAsync();
                }

                if (ListCustomers == null) ListCustomers = new ObservableCollection<KhachHangM>();
                ListCustomers.Insert(0, kh);

                MessageBox.Show("Thêm khách hàng thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        public async void DeleteCustomer(KhachHangM kh)
        {
            try
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xóa khách hàng {kh.HoVaTen}?",
                                             "Xác nhận xóa",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.No) return;

                using (var context = new QuanLyVatTuContext())
                {
                    var itemToDelete = await context.KhachHang
                                            .SingleOrDefaultAsync(x => x.MaKhachHang == kh.MaKhachHang);

                    if (itemToDelete != null)
                    {
                        context.KhachHang.Remove(itemToDelete);
                        await context.SaveChangesAsync();
                    }
                }
                ListCustomers.Remove(kh);

                MessageBox.Show("Xóa thành công!");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE statement conflicted"))
                {
                    MessageBox.Show("Không thể xóa khách hàng này vì họ đã có Hóa đơn hoặc Phiếu thu trong hệ thống!", "Cảnh báo ràng buộc dữ liệu");
                }
                else
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
            }
        }
    }
}