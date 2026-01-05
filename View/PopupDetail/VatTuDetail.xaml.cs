using PageNavigation.Model;
using PageNavigation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
using PageNavigation.Utilities;

namespace PageNavigation.View.PopupDetail
{
    /// <summary>
    /// Interaction logic for VatTuDetail.xaml
    /// </summary>
    public partial class VatTuDetail : Window
    {
        private VatTuVM vm;
        public VatTuM VatTu { get; set; }

        // 2. Biến khớp với {Binding DanhSachLoaiVatTu} trong ComboBox
        public List<LoaiVatTuM> DanhSachLoaiVatTu { get; set; }
        // ===== ĐƠN VỊ TÍNH =====
        public List<DonViTinhM> DanhSachDonViTinh { get; set; }

        private VatTuM _originalVatTu;


        public VatTuDetail(VatTuM vt = null)
        {
            InitializeComponent();

            // --- BƯỚC 1: TẢI DANH SÁCH LOẠI CHO COMBOBOX ---
            using (var context = new QuanLyVatTuContext())
            {
                // Lấy danh sách từ DB đưa vào biến
                DanhSachLoaiVatTu = context.LoaiVatTu.ToList();
                DanhSachDonViTinh = context.DonViTinh.ToList();
            }

            // --- BƯỚC 2: KHỞI TẠO ĐỐI TƯỢNG VẬT TƯ ---
            if (vt == null)
            {
                // Nếu thêm mới -> Tạo cái rỗng
                VatTu = new VatTuM();
            }
            else
            {
                // Nếu sửa -> Lấy cái được truyền vào
                VatTu = vt;
                _originalVatTu = new VatTuM
                {
                    MaVatTu = vt.MaVatTu,
                    TenVatTu = vt.TenVatTu,
                    MaLoai = vt.MaLoai,
                    MaDonViTinh = vt.MaDonViTinh,
                    MoTa = vt.MoTa
                };
            }

            // --- BƯỚC 3: KẾT NỐI DỮ LIỆU (QUAN TRỌNG NHẤT) ---
            // Lệnh này bảo XAML: "Hãy tìm các biến DanhSachLoaiVatTu và VatTu ở ngay trong file này"
            this.DataContext = this;
        }
        // public VatTuDetail(VatTuVM vm)   // <-- truyền ViewModel vào
        // {
        //    InitializeComponent();
        //     this.vm = vm;
        //     DataContext = vm;            // <-- QUAN TRỌNG NHẤT

        // }
        private bool IsDataChanged()
        {
            // Nếu thêm mới → chưa có dữ liệu gốc → KHÔNG coi là thay đổi
            if (_originalVatTu == null || _originalVatTu.MaVatTu == 0)
                return false;

            // So sánh an toàn
            string tenHienTai = VatTu?.TenVatTu?.Trim() ?? "";
            string tenGoc = _originalVatTu?.TenVatTu?.Trim() ?? "";

            string moTaHienTai = VatTu?.MoTa?.Trim() ?? "";
            string moTaGoc = _originalVatTu?.MoTa?.Trim() ?? "";

            if (tenHienTai != tenGoc)
                return true;

            if (VatTu.MaLoai != _originalVatTu.MaLoai)
                return true;

            if (moTaHienTai != moTaGoc)
                return true;

            return false;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            // ===== 1. Không có thay đổi =====
            if (!IsDataChanged())
            {
                MessageBox.Show(
                    "Chưa có dữ liệu nào để đặt lại",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            // ===== 2. Có thay đổi → hỏi =====
            var result = MessageBox.Show(
                "Bạn có muốn đặt lại dữ liệu về trạng thái đã lưu gần nhất không?",
                "Xác nhận đặt lại",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);

            // ===== 3. OK → đặt lại =====
            if (result == MessageBoxResult.OK)
            {
                RestoreOriginalData();
                MessageBox.Show(
                    "Dữ liệu đã được đặt lại về trạng thái đã lưu.",
                    "Hoàn tất",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            // Cancel → không làm gì
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // --- 1. SỬA LỖI SILENT FAILURE (Lỗi bấm không ăn) ---
                // Không được ép kiểu DataContext sang ViewModel nữa!
                // Dùng trực tiếp biến VatTu mà chúng ta đã khai báo ở đầu file.

                if (VatTu == null) VatTu = new VatTuM(); // Phòng hờ null

                // --- 2. KIỂM TRA TÊN (Sửa lỗi "Nhập rồi mà vẫn báo trống") ---
                // Đôi khi Binding chưa kịp cập nhật, ta lấy thẳng từ ô TextBox cho chắc ăn
                // (Giả sử tên TextBox bên XAML là txtTenVatTu)
                if (!string.IsNullOrWhiteSpace(txtTenVatTu.Text))
                {
                    VatTu.TenVatTu = txtTenVatTu.Text; // Gán cứng luôn
                }

                if (string.IsNullOrWhiteSpace(VatTu.TenVatTu))
                {
                    MessageBox.Show("Tên vật tư không được để trống!", "Cảnh báo");
                    return;
                }

                // --- 3. KIỂM TRA LOẠI ---
                if (VatTu.MaLoai <= 0)
                {
                    MessageBox.Show("Vui lòng chọn Loại vật tư!", "Cảnh báo");
                    return;
                }
                if (VatTu.MaDonViTinh <= 0)
                {
                    MessageBox.Show("Vui lòng chọn Đơn vị tính!", "Cảnh báo");
                    return;
                }
                if (txtMoTa != null)
                {
                    VatTu.MoTa = txtMoTa.Text;
                }

                // --- 4. LƯU DATABASE ---
                // --- 4. LƯU DATABASE ---
                using (var db = new QuanLyVatTuContext())
                {
                    // Chuẩn hóa tên trước khi so sánh
                    VatTu.TenVatTu = VatTu.TenVatTu.Trim();

                    // ===== THÊM MỚI =====
                    if (VatTu.MaVatTu == 0)
                    {
                        bool isTrung = db.VatTu.Any(x =>
                            x.TenVatTu.ToLower() == VatTu.TenVatTu.ToLower());

                        if (isTrung)
                        {
                            MessageBox.Show(
                                "Tên vật tư đã tồn tại (không phân biệt chữ hoa/thường)!",
                                "Cảnh báo");
                            return;
                        }

                        db.VatTu.Add(VatTu);
                    }
                    // ===== CẬP NHẬT =====
                    else
                    {
                        bool isTrung = db.VatTu.Any(x =>
                            x.TenVatTu.ToLower() == VatTu.TenVatTu.ToLower()
                            && x.MaVatTu != VatTu.MaVatTu);

                        if (isTrung)
                        {
                            MessageBox.Show(
                                "Tên vật tư đã tồn tại (không phân biệt chữ hoa/thường)!",
                                "Cảnh báo");
                            return;
                        }

                        var item = db.VatTu.FirstOrDefault(x => x.MaVatTu == VatTu.MaVatTu);
                        if (item != null)
                        {
                            item.TenVatTu = VatTu.TenVatTu;
                            item.MaLoai = VatTu.MaLoai;
                            item.MaDonViTinh = VatTu.MaDonViTinh;
                            item.MoTa = VatTu.MoTa;
                        }
                    }

                    db.SaveChanges();
                    GlobalEvents.RaiseVatTuChanged();
                }


                MessageBox.Show("Lưu thành công!");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void RestoreOriginalData()
        {
            if (_originalVatTu == null || VatTu == null)
                return;

            VatTu.TenVatTu = _originalVatTu.TenVatTu;
            VatTu.MaLoai = _originalVatTu.MaLoai;
            VatTu.MoTa = _originalVatTu.MoTa;
            VatTu.MaDonViTinh = _originalVatTu.MaDonViTinh;
            


            // Cập nhật UI ngay lập tức
            txtTenVatTu.Text = VatTu.TenVatTu;
            txtMoTa.Text = VatTu.MoTa;

            // ComboBox loại
            cbLoaiVatTu.SelectedValue = VatTu.MaLoai;
            cbDonViTinh.SelectedValue = VatTu.MaDonViTinh;
        }
    }
}
