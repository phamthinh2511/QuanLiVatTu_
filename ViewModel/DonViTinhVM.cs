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
    public class DonViTinhVM : Utilities.ViewModelBase
    {
		private ObservableCollection<DonViTinhM> _unitCountList;

		public ObservableCollection<DonViTinhM> UnitCountList
		{
			get { return _unitCountList; }
			set { _unitCountList = value; OnPropertyChanged(); }
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
                    var data = await context.DonViTinh.OrderByDescending(x => x.MaDonViTinh).ToListAsync();
                    UnitCountList = new ObservableCollection<DonViTinhM>(data);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
            finally { IsLoading = false; }
        }

        public DonViTinhVM()
        {
            LoadDataAsync();
        }

        public async void AddCountingUnit(DonViTinhM dvt)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    context.DonViTinh.Add(dvt);
                    await context.SaveChangesAsync();
                }

                if (UnitCountList == null) UnitCountList = new ObservableCollection<DonViTinhM>();
                UnitCountList.Insert(0, dvt);

                MessageBox.Show("Thêm đơn vị tính thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        public async void DeleteCountingUnit(DonViTinhM dvt)
        {
            try
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xóa đơn vị tính {dvt.TenDonViTinh}?",
                                             "Xác nhận xóa",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.No) return;

                using (var context = new QuanLyVatTuContext())
                {
                    var itemToDelete = await context.DonViTinh
                                            .SingleOrDefaultAsync(x => x.MaDonViTinh == dvt.MaDonViTinh);

                    if (itemToDelete != null)
                    {
                        context.DonViTinh.Remove(itemToDelete);
                        await context.SaveChangesAsync();
                    }
                }
                UnitCountList.Remove(dvt);

                MessageBox.Show("Xóa thành công!");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE statement conflicted"))
                {
                    MessageBox.Show("Không thể xóa Đơn vị tính này vì đang được sử dụng trong Hóa đơn hoặc Phiếu nhập!",
                                    "Cảnh báo ràng buộc dữ liệu");
                }
                else
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
            }
        }
    }
}
