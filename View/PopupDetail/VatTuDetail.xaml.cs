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

        public VatTuDetail(VatTuM vt = null)
        {
            InitializeComponent();

            // --- BƯỚC 1: TẢI DANH SÁCH LOẠI CHO COMBOBOX ---
            using (var context = new QuanLyVatTuContext())
            {
                // Lấy danh sách từ DB đưa vào biến
                DanhSachLoaiVatTu = context.LoaiVatTu.ToList();
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

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
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
                if (txtMoTa != null)
                {
                    VatTu.MoTa = txtMoTa.Text;
                }

                // --- 4. LƯU DATABASE ---
                using (var db = new QuanLyVatTuContext())
                {
                    if (VatTu.MaVatTu == 0)
                    {
                        // Thêm mới
                        if (db.VatTu.Any(x => x.TenVatTu == VatTu.TenVatTu))
                        {
                            MessageBox.Show("Tên đã tồn tại!");
                            return;
                        }
                        db.VatTu.Add(VatTu);
                    }
                    else
                    {
                        // Cập nhật
                        var item = db.VatTu.FirstOrDefault(x => x.MaVatTu == VatTu.MaVatTu);
                        if (item != null)
                        {
                            item.TenVatTu = VatTu.TenVatTu;
                            item.MaLoai = VatTu.MaLoai;
                            item.MoTa = VatTu.MoTa;
                        }
                    }
                    db.SaveChanges();
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
    }
}
