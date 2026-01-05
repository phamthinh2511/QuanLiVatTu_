using PageNavigation.Model;
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

namespace PageNavigation.View.PopupDetail
{
    /// <summary>
    /// Interaction logic for LoaiVatTuDetail.xaml
    /// </summary>
    public partial class LoaiVatTuDetail : Window
    {
        // Biến lưu trữ đối tượng hiện tại
        public LoaiVatTuM CurrentLoaiVatTu { get; set; }
        private LoaiVatTuM _originalLoaiVatTu;


        // Constructor nhận vào tham số (null = thêm mới, có giá trị = sửa)
        public LoaiVatTuDetail(LoaiVatTuM lvt = null)
        {
            InitializeComponent();

            if (lvt == null)
            {
                // Trường hợp thêm mới -> Tạo object rỗng
                CurrentLoaiVatTu = new LoaiVatTuM();
                _originalLoaiVatTu = null; // thêm mới → không có dữ liệu gốc
            }
            else
            {
                // Trường hợp sửa -> Gán object được truyền vào
                CurrentLoaiVatTu = lvt;
                _originalLoaiVatTu = new LoaiVatTuM
                {
                    MaLoai = lvt.MaLoai,
                    TenLoai = lvt.TenLoai,
                    MoTa = lvt.MoTa
                };

                // Đổ dữ liệu cũ lên giao diện (Nếu không dùng Binding TwoWay)
                txtTenLoai.Text = lvt.TenLoai;
                txtMoTa.Text = lvt.MoTa;
            }

            // Đặt DataContext là chính Window này để Binding hoạt động (nếu cần)
            this.DataContext = this;

            // Gọi kiểm tra ngay khi mở lên để quyết định nút Lưu ẩn hay hiện
            CheckValidate();
        }

        // ----------------------------------------------------
        // Validate dữ liệu (Tương tự Form_Changed)
        // ----------------------------------------------------
        private void Form_Changed(object sender, TextChangedEventArgs e)
        {
            CheckValidate();
        }

        private void CheckValidate()
        {
            // Kiểm tra null các control để tránh lỗi khi khởi tạo
            if (txtTenLoai == null || btnSave == null) return;

            // Kiểm tra Tên loại không được để trống
            bool coTen = !string.IsNullOrWhiteSpace(txtTenLoai.Text);

            // Bật/Tắt nút Lưu dựa trên kết quả kiểm tra
            btnSave.IsEnabled = coTen;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Lấy giá trị từ giao diện
                string tenMoi = txtTenLoai.Text.Trim();
                string moTaMoi = txtMoTa.Text?.Trim();

                using (var context = new QuanLyVatTuContext())
                {
                    // 2. KIỂM TRA TRÙNG TÊN
                    // Tìm xem có ai trùng tên không
                    var loaiTrungTen = context.LoaiVatTu
                        .FirstOrDefault(x => x.TenLoai.ToLower() == tenMoi.ToLower());

                    // Check Logic trùng
                    if (loaiTrungTen != null)
                    {
                        // Nếu là Thêm mới (MaLoai == 0) mà tìm thấy tên -> Lỗi
                        if (CurrentLoaiVatTu.MaLoai == 0)
                        {
                            MessageBox.Show("Tên loại vật tư này đã tồn tại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        // Nếu là Sửa, mà tên trùng đó lại KHÔNG PHẢI là chính mình -> Lỗi
                        if (CurrentLoaiVatTu.MaLoai != 0 && loaiTrungTen.MaLoai != CurrentLoaiVatTu.MaLoai)
                        {
                            MessageBox.Show("Tên loại vật tư này đã tồn tại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    // 3. XỬ LÝ LƯU (Quan trọng nhất)
                    if (CurrentLoaiVatTu.MaLoai == 0)
                    {
                        // === THÊM MỚI ===
                        var newItem = new LoaiVatTuM() // Hoặc LoaiVatTu (tùy tên class trong Model của bạn)
                        {
                            TenLoai = tenMoi,
                            MoTa = moTaMoi
                        };
                        context.LoaiVatTu.Add(newItem);
                    }
                    else
                    {
                        // === SỬA (FIX LỖI KHÔNG LƯU MÔ TẢ & TRACKING) ===
                        // Bước A: Tìm lại thằng cũ trong DB bằng ID
                        var itemInDB = context.LoaiVatTu.FirstOrDefault(x => x.MaLoai == CurrentLoaiVatTu.MaLoai);

                        if (itemInDB != null)
                        {
                            // Bước B: Gán giá trị mới đè lên thằng cũ
                            itemInDB.TenLoai = tenMoi;
                            itemInDB.MoTa = moTaMoi; // <--- Dòng này đảm bảo Mô tả được lưu

                            // Không cần gọi Update(), SaveChanges tự biết itemInDB đã thay đổi
                        }
                    }

                    // 4. Lưu xuống DB
                    context.SaveChanges();
                }

                MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDataChanged())
            {
                MessageBox.Show(
                    "Chưa có dữ liệu nào để đặt lại",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                "Bạn có muốn đặt lại dữ liệu về trạng thái đã lưu gần nhất không?",
                "Xác nhận",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.OK)
            {
                RestoreOriginalData();
            }
        }


        // Đóng form (nút X custom nếu có)
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool IsDataChanged()
        {
            // Thêm mới → không coi là thay đổi
            if (_originalLoaiVatTu == null || _originalLoaiVatTu.MaLoai == 0)
                return false;

            if ((txtTenLoai.Text ?? "").Trim() != (_originalLoaiVatTu.TenLoai ?? "").Trim())
                return true;

            if ((txtMoTa.Text ?? "").Trim() != (_originalLoaiVatTu.MoTa ?? "").Trim())
                return true;

            return false;
        }
        private void RestoreOriginalData()
        {
            if (_originalLoaiVatTu == null) return;

            CurrentLoaiVatTu.TenLoai = _originalLoaiVatTu.TenLoai;
            CurrentLoaiVatTu.MoTa = _originalLoaiVatTu.MoTa;

            txtTenLoai.Text = _originalLoaiVatTu.TenLoai;
            txtMoTa.Text = _originalLoaiVatTu.MoTa;

            CheckValidate();
        }


    }
}