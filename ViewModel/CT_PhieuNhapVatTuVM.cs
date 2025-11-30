using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace PageNavigation.ViewModel
{
    public class CT_PhieuNhapVatTuVM : Utilities.ViewModelBase
    {
        private ObservableCollection<CT_PhieuNhapVatTuM> _receiveNote;

        public ObservableCollection<CT_PhieuNhapVatTuM> ReceiveNote
        {
            get { return _receiveNote; }
            set { _receiveNote = value; OnPropertyChanged(); }
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
                    var data = await context.CT_PhieuNhapVatTu
                        .Include(x => x.MaVatTuNavigation)
                        .Include(x => x.MaDonViTinhNavigation)
                        .Include(x => x.MaPhieuNhapNavigation)
                        .OrderByDescending(x => x.MaPhieuNhap)
                        .ToListAsync();
                    ReceiveNote = new ObservableCollection<CT_PhieuNhapVatTuM>(data);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            finally
            {
                IsLoading = false;
            }
        }

        public CT_PhieuNhapVatTuVM()
        {
            LoadDataAsync();
        }


        public async void AddDetail(CT_PhieuNhapVatTuM detail)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    // 1. Tìm vật tư cần nhập để cập nhật tồn kho
                    var vatTu = await context.VatTu.FindAsync(detail.MaVatTu);

                    if (vatTu != null)
                    {
                        vatTu.SoLuongTon = (vatTu.SoLuongTon ?? 0) + detail.SoLuong;
                    }
                    if (detail.ThanhTien == null || detail.ThanhTien == 0)
                    {
                        detail.ThanhTien = detail.SoLuong * detail.DonGiaNhap;
                    }
                    context.CT_PhieuNhapVatTu.Add(detail);
                    await context.SaveChangesAsync();
                    detail.MaVatTuNavigation = vatTu;
                    var dvt = await context.DonViTinh.FindAsync(detail.MaDonViTinh);
                    detail.MaDonViTinhNavigation = dvt;
                }

                ReceiveNote.Insert(0, detail);
                MessageBox.Show("Nhập hàng thành công! Tồn kho đã tăng.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        public async void DeleteDetail(CT_PhieuNhapVatTuM detail)
        {
            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    var itemToDelete = await context.CT_PhieuNhapVatTu
                        .SingleOrDefaultAsync(x => x.MaPhieuNhap == detail.MaPhieuNhap && x.MaVatTu == detail.MaVatTu);

                    if (itemToDelete != null)
                    {
                        var vatTu = await context.VatTu.FindAsync(detail.MaVatTu);
                        if (vatTu != null)
                        {
                            vatTu.SoLuongTon = (vatTu.SoLuongTon ?? 0) - itemToDelete.SoLuong;
                            if (vatTu.SoLuongTon < 0) vatTu.SoLuongTon = 0;
                        }

                        context.CT_PhieuNhapVatTu.Remove(itemToDelete);
                        await context.SaveChangesAsync();
                    }
                }
                ReceiveNote.Remove(detail);
                MessageBox.Show("Xóa thành công! Đã cập nhật lại tồn kho.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
        }
    }
}