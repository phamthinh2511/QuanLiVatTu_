using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using PageNavigation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PageNavigation.View.PopupDetail
{
    public partial class PhieuNhapVatTuDetail : Window
    {
        // 1. Đối tượng Header (Chứa Ngày nhập)
        public PhieuNhapVatTuM CurrentPhieuNhap { get; set; }

        // 2. Đối tượng Detail (Chứa Vật tư, Nhân viên, Số lượng, Giá...)
        public CT_PhieuNhapVatTuM CurrentChiTiet { get; set; }

        public List<VatTuM> DanhSachVatTu { get; set; }
        public List<NhanVienM> DanhSachNhanVien { get; set; }
        public List<DonViTinhM> DanhSachDonViTinh { get; set; }
        public PhieuNhapVatTuDetail()
        {
            InitializeComponent();
            using (var db = new QuanLyVatTuContext())
            {
                DanhSachVatTu = db.VatTu.ToList();
                DanhSachNhanVien = db.NhanVien.ToList();
                DanhSachDonViTinh = db.DonViTinh.ToList();
            }
            InitForm(null);
        }

        public void InitForm(PhieuNhapVatTuM phieuNhapToEdit = null)
        {
            if (phieuNhapToEdit == null)
            {
                // --- TRƯỜNG HỢP TẠO MỚI ---
                CurrentPhieuNhap = new PhieuNhapVatTuM()
                {
                    NgayNhapPhieu = DateTime.Today,
                    TongTien = 0
                };

                // Tạo mới dòng chi tiết đi kèm
                CurrentChiTiet = new CT_PhieuNhapVatTuM()
                {
                    SoLuong = 1,
                    DonGiaNhap = 0,
                    DonGiaBan = 0,   // Model của bạn có DonGiaBan, nên dùng cái này thay vì TiLe
                    ThanhTien = 0
                };
            }
            else
            {
                // --- TRƯỜNG HỢP SỬA ---
                CurrentPhieuNhap = phieuNhapToEdit;

                using (var db = new QuanLyVatTuContext())
                {
                    // Tìm dòng chi tiết của phiếu này (Lấy dòng đầu tiên tìm được)
                    // Lưu ý: Nếu phiếu có nhiều chi tiết, logic này chỉ lấy cái đầu tiên.
                    var chiTietTimDuoc = db.CT_PhieuNhapVatTu
                                           .FirstOrDefault(x => x.MaPhieuNhap == phieuNhapToEdit.MaPhieuNhap);

                    if (chiTietTimDuoc != null)
                    {
                        CurrentChiTiet = chiTietTimDuoc;
                    }
                    else
                    {
                        // Nếu không tìm thấy chi tiết nào (dữ liệu lỗi), tạo mới để tránh crash
                        CurrentChiTiet = new CT_PhieuNhapVatTuM() { MaPhieuNhap = phieuNhapToEdit.MaPhieuNhap };
                    }
                }
            }

            // GÁN DATACONTEXT
            this.DataContext = this;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validation nhanh
            if (CurrentChiTiet.MaVatTu == 0) { MessageBox.Show("Chưa chọn Vật tư!", "Cảnh báo"); return; }
            if (CurrentChiTiet.MaNhanVien == null || CurrentChiTiet.MaNhanVien == 0) { MessageBox.Show("Chưa chọn Nhân viên!", "Cảnh báo"); return; }
            if (CurrentChiTiet.SoLuong <= 0) { MessageBox.Show("Số lượng phải lớn hơn 0!", "Cảnh báo"); return; }

            try
            {
                using (var db = new QuanLyVatTuContext())
                {
                    if (CurrentPhieuNhap.MaPhieuNhap == 0) // THÊM MỚI
                    {
                        // B1: Lưu Header
                        db.PhieuNhapVatTu.Add(CurrentPhieuNhap);
                        db.SaveChanges(); // Sinh ID tự động

                        // B2: Gán ID vào Detail
                        CurrentChiTiet.MaPhieuNhap = CurrentPhieuNhap.MaPhieuNhap;

                        // B3: Tính thành tiền & Lưu Detail
                        CurrentChiTiet.ThanhTien = CurrentChiTiet.SoLuong * CurrentChiTiet.DonGiaNhap;
                        db.CT_PhieuNhapVatTu.Add(CurrentChiTiet);
                        db.SaveChanges();
                    }
                    else // CẬP NHẬT (Logic đơn giản)
                    {
                        db.PhieuNhapVatTu.Update(CurrentPhieuNhap);

                        CurrentChiTiet.MaPhieuNhap = CurrentPhieuNhap.MaPhieuNhap;
                        CurrentChiTiet.ThanhTien = CurrentChiTiet.SoLuong * CurrentChiTiet.DonGiaNhap;

                        if (db.CT_PhieuNhapVatTu.Any(x => x.MaPhieuNhap == CurrentChiTiet.MaPhieuNhap))
                            db.CT_PhieuNhapVatTu.Update(CurrentChiTiet);
                        else
                            db.CT_PhieuNhapVatTu.Add(CurrentChiTiet);

                        db.SaveChanges();
                    }
                }

                // Báo hiệu reload
                PhieuNhapVatTuService.NotifyChanged();

                MessageBox.Show("Lưu thành công!", "Thông báo");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                // Hiện lỗi chi tiết nếu SQL từ chối
                MessageBox.Show($"Lỗi lưu dữ liệu:\n{ex.Message}\n{ex.InnerException?.Message}", "Lỗi");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) { this.Close(); }
        private void Button_Close(object sender, RoutedEventArgs e) { this.Close(); }
    }
}