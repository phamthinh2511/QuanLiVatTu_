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
    public class CT_HoaDonVM : Utilities.ViewModelBase
    {
		private ObservableCollection<CT_HoaDonM> _billDetail;

		public ObservableCollection<CT_HoaDonM> BillDetail
		{
			get { return _billDetail; }
			set { _billDetail = value; OnPropertyChanged(); }
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
                    var data = await context.CT_HoaDon
                        .Include(x => x.MaVatTuNavigation)
                        .Include(x => x.MaDonViTinhNavigation)
                        .Include(x => x.MaHoaDonNavigation)
                        .OrderByDescending(x => x.MaHoaDon)
                        .ToListAsync();

                    BillDetail = new ObservableCollection<CT_HoaDonM>(data);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            finally
            {
                IsLoading = false;
            }
        }
        public CT_HoaDonVM()
        {
            LoadDataAsync();
        }

        public async void AddDetail(CT_HoaDonM detail)
        {
            if (detail.SoLuongBan <= 0) { MessageBox.Show("Số lượng phải > 0"); return; }
            if (detail.MaVatTu == 0) { MessageBox.Show("Chưa chọn vật tư"); return; }
            if (BillDetail.Any(x => x.MaVatTu == detail.MaVatTu)) { MessageBox.Show("Vật tư đã tồn tại"); return; }
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    var vatTu = await context.VatTu.FindAsync(detail.MaVatTu);

                    if (vatTu == null)
                    {
                        MessageBox.Show("Không tìm thấy vật tư trong hệ thống!");
                        return;
                    }
                    int tonHienTai = vatTu.SoLuongTon ?? 0;
                    if (tonHienTai < detail.SoLuongBan)
                    {
                        MessageBox.Show($"Không đủ hàng để bán! Tồn kho hiện tại: {tonHienTai}", "Cảnh báo hết hàng");
                        return;
                    }
                    vatTu.SoLuongTon = tonHienTai - detail.SoLuongBan;
                    if (detail.ThanhTien == null || detail.ThanhTien == 0)
                        detail.ThanhTien = detail.SoLuongBan * detail.DonGiaBan;
                    context.CT_HoaDon.Add(detail);
                    await context.SaveChangesAsync();
                    var dvt = await context.DonViTinh.FindAsync(detail.MaDonViTinh);
                    detail.MaVatTuNavigation = vatTu;
                    detail.MaDonViTinhNavigation = dvt;
                }

                BillDetail.Insert(0, detail);
                MessageBox.Show("Bán hàng thành công! Tồn kho đã giảm.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        public async void DeleteDetail(CT_HoaDonM detail)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    var itemToDelete = await context.CT_HoaDon
                        .SingleOrDefaultAsync(x => x.MaHoaDon == detail.MaHoaDon && x.MaVatTu == detail.MaVatTu);

                    if (itemToDelete != null)
                    {
                        var vatTu = await context.VatTu.FindAsync(detail.MaVatTu);
                        if (vatTu != null)
                        {
                            vatTu.SoLuongTon = (vatTu.SoLuongTon ?? 0) + itemToDelete.SoLuongBan;
                        }

                        context.CT_HoaDon.Remove(itemToDelete);
                        await context.SaveChangesAsync();
                    }
                }
                BillDetail.Remove(detail);
                MessageBox.Show("Xóa thành công! Đã trả hàng về kho.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
        }
    }
}
