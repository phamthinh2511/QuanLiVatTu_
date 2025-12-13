using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace PageNavigation.View.PopupDetail
{
    public partial class PhieuNhapVatTuDetail : Window, INotifyPropertyChanged
    {
        private PhieuNhapVatTuM _currentPhieuNhap;
        public PhieuNhapVatTuM CurrentPhieuNhap
        {
            get => _currentPhieuNhap;
            set { _currentPhieuNhap = value; OnPropertyChanged(); }
        }

        private CT_PhieuNhapVatTuM _currentChiTiet;
        public CT_PhieuNhapVatTuM CurrentChiTiet
        {
            get => _currentChiTiet;
            set { _currentChiTiet = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CT_PhieuNhapVatTuM> ListChiTietHienThi { get; set; }
        public List<VatTuM> DanhSachVatTu { get; set; }
        public List<NhanVienM> DanhSachNhanVien { get; set; }
        public List<DonViTinhM> DanhSachDonViTinh { get; set; }

        public PhieuNhapVatTuDetail(PhieuNhapVatTuM phieuEdit = null)
        {
            InitializeComponent();
            LoadData();
            InitForm(phieuEdit);
            this.DataContext = this;
        }

        private void LoadData()
        {
            using (var db = new QuanLyVatTuContext())
            {
                DanhSachVatTu = db.VatTu.ToList();
                DanhSachNhanVien = db.NhanVien.ToList();
                DanhSachDonViTinh = db.DonViTinh.ToList();
            }
        }

        public void InitForm(PhieuNhapVatTuM phieuEdit)
        {
            if (phieuEdit == null)
            {
                // THÊM MỚI
                CurrentPhieuNhap = new PhieuNhapVatTuM
                {
                    NgayNhapPhieu = DateTime.Now,
                    TongTien = 0,
                    MaNhanVien = DanhSachNhanVien.FirstOrDefault()?.MaNhanVien // Mặc định
                };
                ListChiTietHienThi = new ObservableCollection<CT_PhieuNhapVatTuM>();
            }
            else
            {
                // SỬA: Load chi tiết cũ
                CurrentPhieuNhap = phieuEdit;
                // Nếu MaNhanVien null (do NotMapped), lấy tạm từ chi tiết đầu tiên để hiển thị lại
                using (var db = new QuanLyVatTuContext())
                {
                    var details = db.CT_PhieuNhapVatTu.Where(x => x.MaPhieuNhap == phieuEdit.MaPhieuNhap).ToList();

                    // Hack: Lấy nhân viên từ dòng chi tiết đầu tiên đắp ngược lên Header để hiển thị
                    if (details.Count > 0 && details[0].MaNhanVien != null)
                        CurrentPhieuNhap.MaNhanVien = details[0].MaNhanVien;

                    foreach (var item in details)
                    {
                        item.TenVatTu = DanhSachVatTu.FirstOrDefault(v => v.MaVatTu == item.MaVatTu)?.TenVatTu;
                        item.TenDonViTinh = DanhSachDonViTinh.FirstOrDefault(d => d.MaDonViTinh == item.MaDonViTinh)?.TenDonViTinh;
                    }
                    ListChiTietHienThi = new ObservableCollection<CT_PhieuNhapVatTuM>(details);
                }
            }
            ResetInput();
            if (ListChiTietHienThi.Count > 0 && btnSave != null)
            {
                btnSave.IsEnabled = true;
            }
        }

        private void ResetInput()
        {
            CurrentChiTiet = new CT_PhieuNhapVatTuM { SoLuong = 1, DonGiaNhap = 0, DonGiaBan = 0, ThanhTien = 0 };
        }

        private void ButtonThem_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentChiTiet.MaVatTu == 0) { MessageBox.Show("Chưa chọn vật tư!"); return; }
            if (CurrentChiTiet.SoLuong <= 0) { MessageBox.Show("Số lượng phải > 0"); return; }

            // 2. Lấy tên hiển thị (Vì ComboBox chỉ lưu ID)
            var vt = DanhSachVatTu.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);
            var dvt = DanhSachDonViTinh.FirstOrDefault(x => x.MaDonViTinh == CurrentChiTiet.MaDonViTinh);

            CurrentChiTiet.TenVatTu = vt?.TenVatTu;
            CurrentChiTiet.TenDonViTinh = dvt?.TenDonViTinh;

            // Tính lại thành tiền
            CurrentChiTiet.ThanhTien = CurrentChiTiet.SoLuong * CurrentChiTiet.DonGiaNhap;

            // 3. KIỂM TRA: Vật tư này đã có trong lưới chưa?
            var existingItem = ListChiTietHienThi.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);

            if (existingItem != null)
            {
                // --- TRƯỜNG HỢP: ĐÃ CÓ -> THÌ SỬA (GHI ĐÈ) ---

                // Tìm vị trí của dòng cũ
                int index = ListChiTietHienThi.IndexOf(existingItem);

                // Thay thế dòng cũ bằng dòng mới (Cách này giúp giao diện tự cập nhật ngay lập tức)
                ListChiTietHienThi[index] = CurrentChiTiet;
            }
            else
            {
                // --- TRƯỜNG HỢP: CHƯA CÓ -> THÌ THÊM MỚI ---
                ListChiTietHienThi.Add(CurrentChiTiet);
            }

            // 4. Cập nhật tổng tiền và Reset form nhập
            UpdateTongTien();

            // Reset lại biến nhập liệu để người dùng nhập món tiếp theo (tránh sửa nhầm món vừa thêm)
            CurrentChiTiet = new CT_PhieuNhapVatTuM { SoLuong = 1, DonGiaNhap = 0, DonGiaBan = 0, ThanhTien = 0 };

            if (btnSave != null) btnSave.IsEnabled = true;
        }

        private void ButtonXoaChon_Click(object sender, RoutedEventArgs e)
        {
            if (lvChiTiet.SelectedItem is CT_PhieuNhapVatTuM item) { ListChiTietHienThi.Remove(item); UpdateTongTien(); }
        }

        private void UpdateTongTien()
        {
            CurrentPhieuNhap.TongTien = ListChiTietHienThi.Sum(x => x.ThanhTien ?? 0);
            OnPropertyChanged(nameof(CurrentPhieuNhap));
        }

        // --- NÚT LƯU ---
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (ListChiTietHienThi.Count == 0) return;
            if (CurrentPhieuNhap.MaNhanVien == null) { MessageBox.Show("Chưa chọn nhân viên!"); return; }

            try
            {
                using (var db = new QuanLyVatTuContext())
                {
                    // 1. LƯU HEADER (Sẽ tự bỏ qua MaNhanVien vì NotMapped)
                    // Reset list chi tiết để tránh lỗi insert nhầm
                    CurrentPhieuNhap.CT_PhieuNhapVatTu = null;

                    if (CurrentPhieuNhap.MaPhieuNhap == 0) db.PhieuNhapVatTu.Add(CurrentPhieuNhap);
                    else db.PhieuNhapVatTu.Update(CurrentPhieuNhap);

                    db.SaveChanges(); // Lấy ID Phiếu mới

                    // 2. LƯU CHI TIẾT (Xóa cũ -> Thêm mới)
                    var oldDetails = db.CT_PhieuNhapVatTu.Where(x => x.MaPhieuNhap == CurrentPhieuNhap.MaPhieuNhap).ToList();
                    db.CT_PhieuNhapVatTu.RemoveRange(oldDetails);
                    db.SaveChanges();

                    foreach (var item in ListChiTietHienThi)
                    {
                        item.MaPhieuNhap = CurrentPhieuNhap.MaPhieuNhap;

                        // ✅ QUAN TRỌNG: Copy nhân viên từ Header xuống Detail
                        item.MaNhanVien = CurrentPhieuNhap.MaNhanVien;

                        // Ngắt quan hệ Object
                        item.MaPhieuNhapNavigation = null;
                        item.MaNhanVienNavigation = null;
                        item.MaVatTuNavigation = null;
                        item.MaDonViTinhNavigation = null;

                        db.CT_PhieuNhapVatTu.Add(item);
                    }
                    db.SaveChanges();
                }

                PageNavigation.Service.PhieuNhapVatTuService.NotifyChanged();
                MessageBox.Show("Lưu thành công!");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show("Lỗi: " + msg);
            }
        }

        private void lvChiTiet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 1. Lấy dòng đang chọn
            var selectedItem = lvChiTiet.SelectedItem as CT_PhieuNhapVatTuM;

            // Nếu click ra ngoài hoặc không chọn gì thì thôi
            if (selectedItem == null) return;

            // 2. COPY dữ liệu từ dòng đó lên Form nhập (Clone object)
            // Phải tạo object mới để ngắt kết nối với dòng cũ dưới lưới
            CurrentChiTiet = new CT_PhieuNhapVatTuM
            {
                MaPhieuNhap = selectedItem.MaPhieuNhap,
                MaVatTu = selectedItem.MaVatTu,
                MaDonViTinh = selectedItem.MaDonViTinh,
                MaNhanVien = selectedItem.MaNhanVien,

                SoLuong = selectedItem.SoLuong,
                DonGiaNhap = selectedItem.DonGiaNhap,
                DonGiaBan = selectedItem.DonGiaBan,
                ThanhTien = selectedItem.ThanhTien,

                // Quan trọng: Phải copy cả tên để ComboBox hiển thị đúng
                TenVatTu = selectedItem.TenVatTu,
                TenDonViTinh = selectedItem.TenDonViTinh
            };
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) => Close();
        private void Button_Close(object sender, RoutedEventArgs e) => Close();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}