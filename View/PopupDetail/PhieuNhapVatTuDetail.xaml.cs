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
        private PhieuNhapVatTuM _phieuNhapBackup;
        private List<CT_PhieuNhapVatTuM> _chiTietBackup;


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

            _phieuNhapBackup = ClonePhieuNhap(CurrentPhieuNhap);
            _chiTietBackup = CloneChiTiet(ListChiTietHienThi);
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
                    // --- 1. XỬ LÝ KHO: HOÀN TÁC SỐ LƯỢNG CŨ (Nếu là Sửa) ---
                    // Logic: Nếu sửa phiếu, ta coi như chưa từng nhập phiếu cũ => Trừ ngược số lượng cũ ra khỏi kho
                    if (CurrentPhieuNhap.MaPhieuNhap != 0)
                    {
                        var oldDetails = db.CT_PhieuNhapVatTu.Where(x => x.MaPhieuNhap == CurrentPhieuNhap.MaPhieuNhap).ToList();

                        foreach (var oldItem in oldDetails)
                        {
                            var vt = db.VatTu.FirstOrDefault(x => x.MaVatTu == oldItem.MaVatTu);
                            if (vt != null)
                            {
                                // TRỪ KHO (Hoàn tác lần nhập trước)
                                vt.SoLuongTon = (vt.SoLuongTon ?? 0) - oldItem.SoLuong;
                            }
                        }

                        // Sau khi trừ kho xong thì xóa chi tiết cũ
                        db.CT_PhieuNhapVatTu.RemoveRange(oldDetails);
                    }

                    // --- 2. LƯU HEADER ---
                    CurrentPhieuNhap.CT_PhieuNhapVatTu = null;
                    if (CurrentPhieuNhap.MaPhieuNhap == 0) db.PhieuNhapVatTu.Add(CurrentPhieuNhap);
                    else db.PhieuNhapVatTu.Update(CurrentPhieuNhap);

                    db.SaveChanges(); // Lưu để chốt ID phiếu

                    // --- 3. LƯU CHI TIẾT MỚI & CỘNG KHO ---
                    foreach (var item in ListChiTietHienThi)
                    {
                        item.MaPhieuNhap = CurrentPhieuNhap.MaPhieuNhap;
                        item.MaNhanVien = CurrentPhieuNhap.MaNhanVien;

                        // Ngắt quan hệ Object
                        item.MaPhieuNhapNavigation = null;
                        item.MaNhanVienNavigation = null;
                        item.MaVatTuNavigation = null;
                        item.MaDonViTinhNavigation = null;

                        // A. Thêm vào bảng chi tiết
                        db.CT_PhieuNhapVatTu.Add(item);

                        // B. CẬP NHẬT KHO (MỚI THÊM ĐOẠN NÀY) 👇
                        var vt = db.VatTu.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                        if (vt != null)
                        {
                            // Logic Nhập hàng: TĂNG số lượng tồn
                            vt.SoLuongTon = (vt.SoLuongTon ?? 0) + item.SoLuong;
                        }
                    }
                    db.SaveChanges(); // Lưu tất cả (Chi tiết + Tồn kho)
                }

                PageNavigation.Service.PhieuNhapVatTuService.NotifyChanged();
                MessageBox.Show("Lưu và cập nhật kho thành công!");
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

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!IsChanged())
            {
                MessageBox.Show(
                    "Chưa có thay đổi nào để hiển thị",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                return;
            }

            var result = MessageBox.Show(
                "Dữ liệu đã thay đổi. Bạn có muốn hoàn tác về lần lưu gần nhất không?",
                "Xác nhận",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.OK)
            {
                RestoreFromBackup();
            }
            // Cancel → không làm gì
        }

        private void Button_Close(object sender, RoutedEventArgs e) => Close();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private PhieuNhapVatTuM ClonePhieuNhap(PhieuNhapVatTuM src)
        {
            if (src == null) return null;

            return new PhieuNhapVatTuM
            {
                MaPhieuNhap = src.MaPhieuNhap,
                NgayNhapPhieu = src.NgayNhapPhieu,
                MaNhanVien = src.MaNhanVien,
                TongTien = src.TongTien
            };
        }

        private List<CT_PhieuNhapVatTuM> CloneChiTiet(IEnumerable<CT_PhieuNhapVatTuM> src)
        {
            return src.Select(x => new CT_PhieuNhapVatTuM
            {
                MaVatTu = x.MaVatTu,
                MaDonViTinh = x.MaDonViTinh,
                MaNhanVien = x.MaNhanVien,
                SoLuong = x.SoLuong,
                DonGiaNhap = x.DonGiaNhap,
                DonGiaBan = x.DonGiaBan,
                ThanhTien = x.ThanhTien,
                TenVatTu = x.TenVatTu,
                TenDonViTinh = x.TenDonViTinh
            }).ToList();
        }
        private bool IsChanged()
        {
            if (_phieuNhapBackup == null) return false;

            if (_phieuNhapBackup.MaNhanVien != CurrentPhieuNhap.MaNhanVien) return true;
            if (_phieuNhapBackup.NgayNhapPhieu != CurrentPhieuNhap.NgayNhapPhieu) return true;
            if (_phieuNhapBackup.TongTien != CurrentPhieuNhap.TongTien) return true;

            if (_chiTietBackup.Count != ListChiTietHienThi.Count) return true;

            for (int i = 0; i < _chiTietBackup.Count; i++)
            {
                var oldItem = _chiTietBackup[i];
                var newItem = ListChiTietHienThi[i];

                if (oldItem.MaVatTu != newItem.MaVatTu) return true;
                if (oldItem.SoLuong != newItem.SoLuong) return true;
                if (oldItem.DonGiaNhap != newItem.DonGiaNhap) return true;
                if (oldItem.DonGiaBan != newItem.DonGiaBan) return true;
            }

            return false;
        }


        private void CboVatTu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 1. Kiểm tra an toàn
            if (CurrentChiTiet == null || DanhSachVatTu == null || CurrentChiTiet.MaVatTu == 0) return;

            // 2. Tìm vật tư vừa chọn trong danh sách
            var vt = DanhSachVatTu.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);

            // 3. TẠO BIẾN TẠM (Để kích hoạt Binding cập nhật giao diện)
            var tempItem = new CT_PhieuNhapVatTuM
            {
                MaPhieuNhap = CurrentChiTiet.MaPhieuNhap,
                MaVatTu = CurrentChiTiet.MaVatTu,
                SoLuong = CurrentChiTiet.SoLuong, // Giữ số lượng đang nhập
                DonGiaNhap = CurrentChiTiet.DonGiaNhap,
                DonGiaBan = CurrentChiTiet.DonGiaBan,
                ThanhTien = 0,

                // Mặc định giữ ĐVT cũ (đề phòng vật tư chưa set ĐVT chuẩn)
                MaDonViTinh = CurrentChiTiet.MaDonViTinh
            };

            if (vt != null)
            {
                tempItem.TenVatTu = vt.TenVatTu;

                // ✅ LOGIC TỰ ĐỘNG CHỌN ĐƠN VỊ TÍNH
                // Lấy trực tiếp từ cài đặt của Vật tư này
                if (vt.MaDonViTinh != null)
                {
                    tempItem.MaDonViTinh = vt.MaDonViTinh;

                    // Lấy tên ĐVT để hiện lên lưới
                    var dvt = DanhSachDonViTinh.FirstOrDefault(d => d.MaDonViTinh == vt.MaDonViTinh);
                    if (dvt != null) tempItem.TenDonViTinh = dvt.TenDonViTinh;
                }
            }

            // 4. (Tùy chọn) Vẫn gợi ý giá nhập từ lần cuối (nếu muốn)
            // Bạn có thể giữ lại đoạn code query lịch sử giá nhập ở đây nếu cần...

            // 5. Gán ngược lại vào biến chính -> Giao diện tự nhảy ĐVT
            CurrentChiTiet = tempItem;
        }

        private void RestoreFromBackup()
        {
            CurrentPhieuNhap = ClonePhieuNhap(_phieuNhapBackup);
            ListChiTietHienThi = new ObservableCollection<CT_PhieuNhapVatTuM>(
                CloneChiTiet(_chiTietBackup)
            );

            OnPropertyChanged(nameof(CurrentPhieuNhap));
            OnPropertyChanged(nameof(ListChiTietHienThi));

            ResetInput();
        }



    }
}