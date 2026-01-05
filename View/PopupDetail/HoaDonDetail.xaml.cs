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
        // ===== BACKUP DỮ LIỆU LẦN LƯU GẦN NHẤT =====
        private HoaDonM _hoaDonBackup;
        private ObservableCollection<CT_HoaDonM> _chiTietBackup;

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
            _hoaDonBackup = CloneHoaDon(CurrentHoaDon);
            _chiTietBackup = CloneChiTiet(ListChiTietHienThi);
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

            // --- 👇 ĐOẠN LOGIC KIỂM TRA TỒN KHO THÔNG MINH 👇 ---

            // 1. Lấy tồn kho hiện tại (đang là 5)
            var vtKho = DanhSachVatTu.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);
            if (vtKho != null)
            {
                int tonKhoHienTai = vtKho.SoLuongTon ?? 0;
                int soLuongDaMuaCu = 0;

                // 2. Nếu đang sửa hóa đơn cũ, đi tìm xem "Hồi xưa mình đã mua bao nhiêu?"
                if (CurrentHoaDon.MaHoaDon != 0)
                {
                    using (var db = new QuanLyVatTuContext())
                    {
                        // Tìm dòng chi tiết cũ trong DB
                        var itemCu = db.CT_HoaDon.FirstOrDefault(x =>
                                        x.MaHoaDon == CurrentHoaDon.MaHoaDon &&
                                        x.MaVatTu == CurrentChiTiet.MaVatTu);

                        if (itemCu != null)
                        {
                            soLuongDaMuaCu = itemCu.SoLuongBan; // Ví dụ: 10
                        }
                    }
                }

                // 3. Tính tổng khả năng cung cấp
                // (Kho 5 + Đang giữ 10 = Có thể bán tối đa 15)
                int tongCoTheBan = tonKhoHienTai + soLuongDaMuaCu;

                // 4. Tính tổng khách muốn mua
                int khachMuonMua = CurrentChiTiet.SoLuongBan; // Ví dụ: 15

                // Nếu đang thêm mới vào lưới (chưa phải sửa dòng), phải cộng dồn với số đã có trên lưới
                if (_itemDangSua == null)
                {
                    var daCoTrenLuoi = ListChiTietHienThi.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);
                    if (daCoTrenLuoi != null) khachMuonMua += daCoTrenLuoi.SoLuongBan;
                }

                // 5. SO SÁNH
                if (khachMuonMua > tongCoTheBan)
                {
                    MessageBox.Show($"Kho hiện tại: {tonKhoHienTai}\n" +
                                    $"Hóa đơn đang giữ: {soLuongDaMuaCu}\n" +
                                    $"-> Tối đa có thể bán: {tongCoTheBan}.\n\n" +
                                    $"Bạn đang nhập quá số lượng ({khachMuonMua})!",
                                    "Không đủ hàng", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            // --- 👆 HẾT PHẦN KIỂM TRA KHO 👆 ---


            // ... (Phần logic thêm vào lưới bên dưới giữ nguyên như cũ) ...

            // Map tên hiển thị
            CurrentChiTiet.TenVatTu = vtKho?.TenVatTu;
            var dvt = DanhSachDonViTinh.FirstOrDefault(x => x.MaDonViTinh == CurrentChiTiet.MaDonViTinh);
            CurrentChiTiet.TenDonViTinh = dvt?.TenDonViTinh;

            // Tính tiền
            CurrentChiTiet.ThanhTien = CurrentChiTiet.SoLuongBan * CurrentChiTiet.DonGiaBan;

            if (_itemDangSua == null)
            {
                // THÊM MỚI
                var exist = ListChiTietHienThi.FirstOrDefault(x => x.MaVatTu == CurrentChiTiet.MaVatTu);
                if (exist != null)
                {
                    exist.SoLuongBan += CurrentChiTiet.SoLuongBan;
                    exist.ThanhTien = exist.SoLuongBan * exist.DonGiaBan;
                    var idx = ListChiTietHienThi.IndexOf(exist);
                    ListChiTietHienThi[idx] = exist;
                }
                else
                {
                    ListChiTietHienThi.Add(CurrentChiTiet);
                }
            }
            else
            {
                // CẬP NHẬT
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
            CT_HoaDonM itemCanXoa = _itemDangSua;

            // Ưu tiên 2: Nếu _itemDangSua null, thử lấy từ dòng đang chọn trên ListView (Phòng hờ)
            if (itemCanXoa == null)
            {
                itemCanXoa = lvChiTiet.SelectedItem as CT_HoaDonM;
            }

            // THỰC HIỆN XÓA
            if (itemCanXoa != null)
            {
                // Xóa khỏi danh sách hiển thị
                // (Lệnh này sẽ tự động cập nhật UI vì là ObservableCollection)
                ListChiTietHienThi.Remove(itemCanXoa);

                // Cập nhật lại tổng tiền
                UpdateTongTien();

                // Xóa trắng form và hủy chế độ sửa
                ResetInput();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vật tư cần xóa!");
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
            if (CurrentHoaDon.MaKhachHang == null) { MessageBox.Show("Chưa chọn khách hàng!"); return; }
            if (CurrentHoaDon.MaNhanVien == null) { MessageBox.Show("Chưa chọn nhân viên!"); return; }

            try
            {
                using (var db = new QuanLyVatTuContext())
                {
                    // --- BƯỚC 1: XỬ LÝ KHO KHI SỬA HÓA ĐƠN CŨ ---
                    // Nếu đang sửa hóa đơn, ta phải HOÀN TRẢ số lượng cũ về kho trước
                    if (CurrentHoaDon.MaHoaDon != 0)
                    {
                        var chiTietCu = db.CT_HoaDon.Where(x => x.MaHoaDon == CurrentHoaDon.MaHoaDon).ToList();

                        foreach (var itemCu in chiTietCu)
                        {
                            var vt = db.VatTu.FirstOrDefault(x => x.MaVatTu == itemCu.MaVatTu);
                            if (vt != null)
                            {
                                // Logic Bán: Hồi xưa bán đi (Trừ), giờ sửa lại thì phải trả lại kho (Cộng)
                                vt.SoLuongTon = (vt.SoLuongTon ?? 0) + itemCu.SoLuongBan;
                            }
                        }

                        // Xóa chi tiết cũ trong DB
                        db.CT_HoaDon.RemoveRange(chiTietCu);
                    }

                    // --- BƯỚC 2: LƯU HEADER (HÓA ĐƠN) ---
                    // Ngắt quan hệ object để tránh lỗi
                    CurrentHoaDon.CT_HoaDon = null;
                    CurrentHoaDon.MaNhanVienNavigation = null;
                    CurrentHoaDon.MaKhachHangNavigation = null;
                    try { ((dynamic)CurrentHoaDon).MaNhanVienNavigation = null; } catch { }
                    try { ((dynamic)CurrentHoaDon).MaKhachHangNavigation = null; } catch { }

                    if (CurrentHoaDon.MaHoaDon == 0) db.HoaDon.Add(CurrentHoaDon);
                    else db.HoaDon.Update(CurrentHoaDon);

                    db.SaveChanges(); // Lưu để sinh ID hóa đơn

                    // --- BƯỚC 3: LƯU CHI TIẾT MỚI & TRỪ KHO ---
                    foreach (var item in ListChiTietHienThi)
                    {
                        item.MaHoaDon = CurrentHoaDon.MaHoaDon;

                        // Ngắt quan hệ object
                        item.MaHoaDonNavigation = null;
                        item.MaVatTuNavigation = null;
                        item.MaDonViTinhNavigation = null;

                        // A. Thêm vào bảng chi tiết
                        db.CT_HoaDon.Add(item);

                        // B. TRỪ KHO (Logic chính của Bán Hàng) 👇
                        var vt = db.VatTu.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                        if (vt != null)
                        {
                            // Logic Bán: Bán đi thì kho phải GIẢM
                            vt.SoLuongTon = (vt.SoLuongTon ?? 0) - item.SoLuongBan;
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
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            // 1. Không có thay đổi
            if (!IsChanged())
            {
                MessageBox.Show("Chưa có thay đổi nào để hiển thị",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return;
            }

            // 2. Có thay đổi → Hỏi xác nhận
            var result = MessageBox.Show(
                "Bạn có muốn đặt lại dữ liệu về lần lưu gần nhất không?",
                "Xác nhận",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);

            // 3. OK → Restore
            if (result == MessageBoxResult.OK)
            {
                CurrentHoaDon = CloneHoaDon(_hoaDonBackup);
                ListChiTietHienThi = CloneChiTiet(_chiTietBackup);

                OnPropertyChanged(nameof(CurrentHoaDon));
                OnPropertyChanged(nameof(ListChiTietHienThi));

                ResetInput();
            }
            // Cancel → Không làm gì
        }

        private void Button_Close(object sender, RoutedEventArgs e) => Close();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private HoaDonM CloneHoaDon(HoaDonM source)
        {
            if (source == null) return null;

            return new HoaDonM
            {
                MaHoaDon = source.MaHoaDon,
                NgayLapHoaDon = source.NgayLapHoaDon,
                MaNhanVien = source.MaNhanVien,
                MaKhachHang = source.MaKhachHang,
                TongTien = source.TongTien
            };
        }

        private ObservableCollection<CT_HoaDonM> CloneChiTiet(IEnumerable<CT_HoaDonM> source)
        {
            return new ObservableCollection<CT_HoaDonM>(
                source.Select(x => new CT_HoaDonM
                {
                    MaHoaDon = x.MaHoaDon,
                    MaVatTu = x.MaVatTu,
                    MaDonViTinh = x.MaDonViTinh,
                    SoLuongBan = x.SoLuongBan,
                    DonGiaBan = x.DonGiaBan,
                    ThanhTien = x.ThanhTien,
                    TenVatTu = x.TenVatTu,
                    TenDonViTinh = x.TenDonViTinh
                })
            );
        }
        private bool IsChanged()
        {
            if (_hoaDonBackup == null) return false;

            // So sánh Header
            if (CurrentHoaDon.MaNhanVien != _hoaDonBackup.MaNhanVien) return true;
            if (CurrentHoaDon.MaKhachHang != _hoaDonBackup.MaKhachHang) return true;
            if (CurrentHoaDon.NgayLapHoaDon != _hoaDonBackup.NgayLapHoaDon) return true;

            // So sánh chi tiết
            if (ListChiTietHienThi.Count != _chiTietBackup.Count) return true;

            for (int i = 0; i < ListChiTietHienThi.Count; i++)
            {
                var cur = ListChiTietHienThi[i];
                var old = _chiTietBackup[i];

                if (cur.MaVatTu != old.MaVatTu) return true;
                if (cur.SoLuongBan != old.SoLuongBan) return true;
                if (cur.DonGiaBan != old.DonGiaBan) return true;
            }

            return false;
        }




    }
}
