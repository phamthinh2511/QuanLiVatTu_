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
    public class VatTuVM : Utilities.ViewModelBase
    {
        private ObservableCollection<VatTuM> _listProducts;
        public ObservableCollection<VatTuM> ListProducts
        {
            get { return _listProducts; }
            set { _listProducts = value; OnPropertyChanged(); }
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
                    var data = await context.VatTu.OrderByDescending(x => x.MaVatTu).ToListAsync();
                    ListProducts = new ObservableCollection<VatTuM>(data);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
            finally { IsLoading = false; }
        }

        public VatTuVM()
        {
            LoadDataAsync();
        }

        public async void AddCustomer(VatTuM vt)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    context.VatTu.Add(vt);
                    await context.SaveChangesAsync();
                }

                if (ListProducts == null) ListProducts = new ObservableCollection<VatTuM>();
                ListProducts.Insert(0, vt);

                MessageBox.Show("Thêm vật tư thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        public async void DeleteCustomer(VatTuM vt)
        {
            try
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xóa vật tư {vt.TenVatTu}?",
                                             "Xác nhận xóa",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.No) return;

                using (var context = new QuanLyVatTuContext())
                {
                    var itemToDelete = await context.VatTu
                                            .SingleOrDefaultAsync(x => x.MaVatTu == vt.MaVatTu);

                    if (itemToDelete != null)
                    {
                        context.VatTu.Remove(itemToDelete);
                        await context.SaveChangesAsync();
                    }
                }
                ListProducts.Remove(vt);

                MessageBox.Show("Xóa thành công!");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE statement conflicted"))
                {
                    MessageBox.Show("Không thể xóa vật tư này vì đã có trong Hóa đơn hoặc Phiếu thu trong hệ thống!", "Cảnh báo ràng buộc dữ liệu");
                }
                else
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
            }
        }
    }
}
