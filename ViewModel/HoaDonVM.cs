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
    public class HoaDonVM : Utilities.ViewModelBase
    {
		private ObservableCollection<HoaDonM> _billList;

		public ObservableCollection<HoaDonM> BillList
		{
			get { return _billList; }
			set { _billList = value; OnPropertyChanged(); }
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
                    var data = await context.HoaDon.OrderByDescending(x => x.MaHoaDon).ToListAsync();
                    BillList = new ObservableCollection<HoaDonM>(data);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            finally
            {
                IsLoading = false;
            }
        }
        public HoaDonVM()
        {
            LoadDataAsync();
        }
        public async void AddDetail(HoaDonM bill)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    context.HoaDon.Add(bill);
                    await context.SaveChangesAsync();
                }
                BillList.Insert(0, bill);
                MessageBox.Show("Thêm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }
        public async void DeleteBill(HoaDonM bill)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    var itemToDelete = await context.HoaDon
                        .Include(h => h.CT_HoaDon)
                        .SingleOrDefaultAsync(x => x.MaHoaDon == bill.MaHoaDon);

                    if (itemToDelete != null)
                    {
                        if (itemToDelete.CT_HoaDon != null && itemToDelete.CT_HoaDon.Any())
                        {
                            context.CT_HoaDon.RemoveRange(itemToDelete.CT_HoaDon);
                        }
                        context.HoaDon.Remove(itemToDelete);
                        await context.SaveChangesAsync();
                    }
                }
                BillList.Remove(bill);
                MessageBox.Show("Xóa thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
