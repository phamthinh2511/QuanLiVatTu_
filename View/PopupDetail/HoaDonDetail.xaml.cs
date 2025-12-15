using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PageNavigation.View.PopupDetail
{
    /// <summary>
    /// Interaction logic for HoaDonDetail.xaml
    /// </summary>
    public partial class HoaDonDetail : Window, INotifyPropertyChanged
    {
        // 1. Header (Hóa Đơn)
        private HoaDonM _currentHoaDon;
        public HoaDonM CurrentHoaDon
        {
            get => _currentHoaDon;
            set { _currentHoaDon = value; OnPropertyChanged(); }
        }

        // 2. Detail Input (Dòng đang nhập)
        private CT_HoaDonM _currentChiTiet;
        public CT_HoaDonM CurrentChiTiet
        {
            get => _currentChiTiet;
            set { _currentChiTiet = value; OnPropertyChanged(); }
        }

        // Biến nhớ dòng đang sửa trên lưới
        private CT_HoaDonM _itemDangSua = null;

        // Các danh sách nguồn cho ComboBox
        public ObservableCollection<CT_HoaDonM> ListChiTietHienThi { get; set; }
        public List<VatTuM> DanhSachVatTu { get; set; }
        public List<NhanVienM> DanhSachNhanVien { get; set; }
        public List<KhachHangM> DanhSachKhachHang { get; set; }
        public List<DonViTinhM> DanhSachDonViTinh { get; set; }

        public HoaDonDetail(HoaDonM hoaDonEdit = null)
        {
            InitializeComponent();
            LoadComboBoxData();
            InitForm(hoaDonEdit);
            this.DataContext = this;
        }

        private void LoadComboBoxData()
        {
            using (var db = new QuanLyVatTuContext())
            {
                DanhSachVatTu = db.VatTu.ToList();
                DanhSachNhanVien = db.NhanVien.ToList();
                DanhSachKhachHang = db.KhachHang.ToList();
                DanhSachDonViTinh = db.DonViTinh.ToList();
            }
        }

        public void InitForm(HoaDonM hoaDonEdit)
        {
            if (hoaDonEdit == null)
            {
                // TẠO MỚI
                CurrentHoaDon = new HoaDonM
                {
                    NgayLapHoaDon = DateTime.Now,
                    TongTien = 0,
                    // Mặc định chọn NV đầu tiên
                    MaNhanVien = DanhSachNhanVien.FirstOrDefault()?.MaNhanVien
                };
                ListChiTietHienThi = new ObservableCollection<CT_HoaDonM>();
            }
            else
            {
                // SỬA: Load dữ liệu cũ
                CurrentHoaDon = hoaDonEdit;
                using (var db = new QuanLyVatTuContext())
                {
                    var details = db.CT_HoaDon.Where(x => x.MaHoaDon == hoaDonEdit.MaHoaDon).ToList();

                    // Map tên hiển thị (Vì trong DB chỉ có ID)
                    foreach (var item in details)
                    {
                        item.TenVatTu = DanhSachVatTu.FirstOrDefault(v => v.MaVatTu == item.MaVatTu)?.TenVatTu;
                        item.TenDonViTinh = DanhSachDonViTinh.FirstOrDefault(d => d.MaDonViTinh == item.MaDonViTinh)?.TenDonViTinh;
                    }
                    ListChiTietHienThi = new ObservableCollection<CT_HoaDonM>(details);
                }
            }
            ResetInput();

            // Nếu đang sửa thì bật nút Lưu luôn
            if (ListChiTietHienThi.Count > 0 && btnSave != null) btnSave.IsEnabled = true;
        }

        private void ResetInput()
        {
            CurrentChiTiet = new CT_HoaDonM { SoLuongBan = 1, DonGiaBan = 0, ThanhTien = 0 };
            _itemDangSua = null;
            if (lvChiTiet != null) lvChiTiet.SelectedItem = null;
        }

        // --- NÚT THÊM VÀO LƯỚI ---
        private void ButtonThem_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentChiTiet.MaVatTu == 0) { MessageBox.Show("Chưa chọn vật tư!"); return; }
            if (CurrentChiTiet.SoLuongBan <= 0) { MessageBox.Show("Số lượng phải > 0"); return; }

            // 1. Lấy thông tin vật tư từ danh sách nguồn để kiểm tra tồn kho
            var vtTrongKho = DanhSachVatTu.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);
            if (vtTrongKho == null) return;

            // 2. Tính tổng số lượng người dùng ĐANG MUỐN MUA
            int soLuongMuonMua = CurrentChiTiet.SoLuongBan;

            // Nếu đang thêm mới (không phải sửa), cần kiểm tra xem trong lưới đã có món này chưa để cộng dồn
            if (_itemDangSua == null)
            {
                var daCoTrongLuoi = ListChiTietHienThi.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);
                if (daCoTrongLuoi != null)
                {
                    soLuongMuonMua += daCoTrongLuoi.SoLuongBan; // Cộng dồn số cũ + số mới
                }
            }

            // 3. SO SÁNH VỚI TỒN KHO (Logic quan trọng nhất)
            // Lưu ý: VatTuM cần có thuộc tính SoLuongTon (kiểm tra Model của bạn)
            if (soLuongMuonMua > (vtTrongKho.SoLuongTon ?? 0))
            {
                MessageBox.Show($"Kho chỉ còn {vtTrongKho.SoLuongTon} {vtTrongKho.TenDonViTinh}.\nBạn không thể bán quá số lượng tồn!",
                                "Cảnh báo hết hàng", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Dừng ngay, không cho chạy tiếp xuống dưới
            }


            // 1. Lấy tên hiển thị
            var vt = DanhSachVatTu.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);
            var dvt = DanhSachDonViTinh.FirstOrDefault(x => x.MaDonViTinh == CurrentChiTiet.MaDonViTinh);
            CurrentChiTiet.TenVatTu = vt?.TenVatTu;
            CurrentChiTiet.TenDonViTinh = dvt?.TenDonViTinh;

            // 2. Tính thành tiền
            CurrentChiTiet.ThanhTien = CurrentChiTiet.SoLuongBan * CurrentChiTiet.DonGiaBan;

            if (_itemDangSua == null)
            {
                // THÊM MỚI (Cộng dồn nếu trùng vật tư)
                var exist = ListChiTietHienThi.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);
                if (exist != null)
                {
                    exist.SoLuongBan += CurrentChiTiet.SoLuongBan;
                    exist.ThanhTien = exist.SoLuongBan * exist.DonGiaBan;
                    var idx = ListChiTietHienThi.IndexOf(exist);
                    ListChiTietHienThi[idx] = exist; // Cập nhật UI
                }
                else
                {
                    ListChiTietHienThi.Add(CurrentChiTiet);
                }
            }
            else
            {
                // CẬP NHẬT (Sửa dòng đang chọn)
                var idx = ListChiTietHienThi.IndexOf(_itemDangSua);
                if (idx != -1) ListChiTietHienThi[idx] = CurrentChiTiet;
            }

            UpdateTongTien();
            ResetInput();
            if (btnSave != null) btnSave.IsEnabled = true;
        }

        private void CboVatTu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Chặn lỗi khi form chưa load xong
            if (CurrentChiTiet == null || DanhSachVatTu == null) return;

            // Nếu chưa chọn gì thì thoát
            if (CurrentChiTiet.MaVatTu == 0) return;

            // 1. Gán Tên hiển thị
            var vt = DanhSachVatTu.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);
            if (vt != null)
            {
                CurrentChiTiet.TenVatTu = vt.TenVatTu;
                CurrentChiTiet.TenDonViTinh = vt.TenDonViTinh; // (Nếu bạn đã thêm NotMapped vào VatTuM)
            }

            // 2. TÌM GIÁ & ĐVT TRONG LỊCH SỬ NHẬP
            using (var db = new QuanLyVatTuContext())
            {
                var lanNhapGanNhat = db.CT_PhieuNhapVatTu
                                       .Where(x => x.MaVatTu == CurrentChiTiet.MaVatTu)
                                       .OrderByDescending(x => x.MaPhieuNhap)
                                       .FirstOrDefault();

                if (lanNhapGanNhat != null)
                {
                    // Lấy giá bán
                    CurrentChiTiet.DonGiaBan = lanNhapGanNhat.DonGiaBan ?? 0;

                    // Lấy Đơn vị tính
                    if (lanNhapGanNhat.MaDonViTinh != null)
                    {
                        CurrentChiTiet.MaDonViTinh = lanNhapGanNhat.MaDonViTinh;
                    }
                }
                else
                {
                    CurrentChiTiet.DonGiaBan = 0;
                }
            }

            // 3. Tính thành tiền
            CurrentChiTiet.ThanhTien = CurrentChiTiet.SoLuongBan * CurrentChiTiet.DonGiaBan;

            // --- QUAN TRỌNG NHẤT: ÉP GIAO DIỆN CẬP NHẬT ---
            // Vì CT_HoaDonM không tự báo thay đổi, ta phải báo cho nó biết
            // là "Toàn bộ object CurrentChiTiet đã thay đổi, hãy load lại textbox đi"
            OnPropertyChanged(nameof(CurrentChiTiet));
        }

        // --- SỰ KIỆN CHỌN DÒNG ĐỂ SỬA ---
        private void lvChiTiet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = lvChiTiet.SelectedItem as CT_HoaDonM;
            if (item == null) return;

            _itemDangSua = item;

            // 1. Clone dữ liệu
            var tempItem = new CT_HoaDonM
            {
                MaHoaDon = item.MaHoaDon,
                MaVatTu = item.MaVatTu,
                MaDonViTinh = item.MaDonViTinh,
                SoLuongBan = item.SoLuongBan,
                DonGiaBan = item.DonGiaBan,
                ThanhTien = item.ThanhTien,
                TenVatTu = item.TenVatTu,
                TenDonViTinh = item.TenDonViTinh
            };

            // 2. Gán vào biến chính
            CurrentChiTiet = tempItem;

            // 3. ÉP GIAO DIỆN CẬP NHẬT (Dòng này cực quan trọng)
            // Nó sẽ kích hoạt sự kiện PropertyChanged của biến CurrentChiTiet
            // làm cho tất cả TextBox đang binding vào CurrentChiTiet phải load lại giá trị mới
            OnPropertyChanged(nameof(CurrentChiTiet));
        }

        private void ButtonXoaChon_Click(object sender, RoutedEventArgs e)
        {
            if (lvChiTiet.SelectedItem is CT_HoaDonM item)
            {
                ListChiTietHienThi.Remove(item);
                UpdateTongTien();
                ResetInput();
            }
        }

        private void UpdateTongTien()
        {
            CurrentHoaDon.TongTien = ListChiTietHienThi.Sum(x => x.ThanhTien ?? 0);
            OnPropertyChanged(nameof(CurrentHoaDon));
        }

        // --- NÚT LƯU QUAN TRỌNG NHẤT ---
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (ListChiTietHienThi.Count == 0) return;
            if (CurrentHoaDon.MaNhanVien == null) { MessageBox.Show("Chưa chọn nhân viên!"); return; }
            if (CurrentHoaDon.MaKhachHang == null) { MessageBox.Show("Chưa chọn khách hàng!"); return; }

            try
            {
                using (var db = new QuanLyVatTuContext())
                {
                    // --- 1. LOGIC CẬP NHẬT KHO (MỚI THÊM) ---

                    // Nếu là SỬA hóa đơn cũ: Phải HOÀN TRẢ lại số lượng cũ vào kho trước
                    if (CurrentHoaDon.MaHoaDon != 0)
                    {
                        var chiTietCu = db.CT_HoaDon.Where(x => x.MaHoaDon == CurrentHoaDon.MaHoaDon).ToList();
                        foreach (var itemCu in chiTietCu)
                        {
                            var vt = db.VatTu.FirstOrDefault(x => x.MaVatTu == itemCu.MaVatTu);
                            if (vt != null)
                            {
                                vt.SoLuongTon += itemCu.SoLuongBan; // Trả lại kho
                            }
                        }
                    }

                    // --- 2. LƯU HEADER (Giữ nguyên) ---
                    CurrentHoaDon.CT_HoaDon = null;
                    CurrentHoaDon.MaNhanVienNavigation = null;
                    CurrentHoaDon.MaKhachHangNavigation = null;
                    try { ((dynamic)CurrentHoaDon).MaNhanVienNavigation = null; } catch { }
                    try { ((dynamic)CurrentHoaDon).MaKhachHangNavigation = null; } catch { }

                    if (CurrentHoaDon.MaHoaDon == 0) db.HoaDon.Add(CurrentHoaDon);
                    else db.HoaDon.Update(CurrentHoaDon);
                    db.SaveChanges();

                    // --- 3. LƯU DETAIL & TRỪ KHO MỚI (SỬA ĐỔI) ---

                    // Xóa chi tiết cũ trong DB
                    var oldDetails = db.CT_HoaDon.Where(x => x.MaHoaDon == CurrentHoaDon.MaHoaDon).ToList();
                    db.CT_HoaDon.RemoveRange(oldDetails);

                    foreach (var item in ListChiTietHienThi)
                    {
                        item.MaHoaDon = CurrentHoaDon.MaHoaDon;

                        // Ngắt quan hệ
                        item.MaHoaDonNavigation = null; item.MaVatTuNavigation = null; item.MaDonViTinhNavigation = null;

                        // A. Thêm vào bảng chi tiết hóa đơn
                        db.CT_HoaDon.Add(item);

                        // B. TRỪ KHO (Logic quan trọng)
                        var vatTuTrongKho = db.VatTu.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                        if (vatTuTrongKho != null)
                        {
                            // Trừ đi số lượng bán
                            vatTuTrongKho.SoLuongTon -= item.SoLuongBan;
                        }
                    }

                    db.SaveChanges(); // Lưu tất cả thay đổi (Hóa đơn + Kho)
                }

                MessageBox.Show("Lưu hóa đơn và Cập nhật kho thành công!");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show("Lỗi: " + msg);
            }
        }

        


        // Boilerplate code
        private void ButtonCancel_Click(object sender, RoutedEventArgs e) => Close();
        private void Button_Close(object sender, RoutedEventArgs e) => Close();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
