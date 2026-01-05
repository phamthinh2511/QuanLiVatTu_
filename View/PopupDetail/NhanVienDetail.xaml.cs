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
    /// Interaction logic for NhanVienDetail.xaml
    /// </summary>
    
    public partial class NhanVienDetail : Window
    {
        public NhanVienM CurrentEmployee { get; set; }
        private NhanVienM _originalEmployee;

        public NhanVienDetail(NhanVienM nv = null)
        {
            InitializeComponent();
            if (nv == null)
            {
                CurrentEmployee = new NhanVienM();
                _originalEmployee = null;
                //CurrentEmployee.NgaySinh = DateOnly.FromDateTime(DateTime.Now);
                // CurrentEmployee.NgayNhanViec = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                CurrentEmployee = nv;
                _originalEmployee = new NhanVienM
                {
                    MaNhanVien = nv.MaNhanVien,
                    HoTen = nv.HoTen,
                    SoDienThoai = nv.SoDienThoai,
                    Username = nv.Username,
                    Password = nv.Password,
                    NgaySinh = nv.NgaySinh,
                    NgayNhanViec = nv.NgayNhanViec,
                    ChucVu = nv.ChucVu
                };
            }

            this.DataContext = this;
        }
        // --------------------------------------------
    // 1) Wrapper cho NgaySinh
    // --------------------------------------------
    public DateTime? NgaySinhDateTime
        {
            get => CurrentEmployee.NgaySinh?.ToDateTime(TimeOnly.MinValue);
            set
            {
                CurrentEmployee.NgaySinh = value.HasValue
                    ? DateOnly.FromDateTime(value.Value)
                    : (DateOnly?)null;
            }
        }

        // --------------------------------------------
        // 2) Wrapper cho NgayNhanViec
        // --------------------------------------------
        public DateTime? NgayNhanViecDateTime
        {
            get => CurrentEmployee.NgayNhanViec?.ToDateTime(TimeOnly.MinValue);
            set
            {
                CurrentEmployee.NgayNhanViec = value.HasValue
                    ? DateOnly.FromDateTime(value.Value)
                    : (DateOnly?)null;
            }
        }
        // Validate dữ liệu
        // ----------------------------------------------------
        private void Form_Changed(object sender, RoutedEventArgs e)
        {
            CheckValidate();
        }

        private void CheckValidate()
        {
            if (txtHoTen == null || txtSDT == null ||
                txtUsername == null || txtPassword == null ||
                btnSave == null) return;

            bool coTen = !string.IsNullOrWhiteSpace(txtHoTen.Text);
            bool coSDT = !string.IsNullOrWhiteSpace(txtSDT.Text);
            bool coUser = !string.IsNullOrWhiteSpace(txtUsername.Text);
            bool coPass = !string.IsNullOrWhiteSpace(txtPassword.Password);

            btnSave.IsEnabled = coTen && coSDT && coUser && coPass;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateEmployee())
                return;
            try
            {
                // -------------------------------
                // 1) GÁN GIÁ TRỊ TỪ UI VÀO MODEL
                // ------------------------------- 
                CurrentEmployee.HoTen = txtHoTen.Text;
                CurrentEmployee.SoDienThoai = txtSDT.Text;
                CurrentEmployee.Username = txtUsername.Text;
                CurrentEmployee.Password = txtPassword.Password;

                // Ngày sinh
                if (dpNgaySinh.SelectedDate.HasValue)
                    CurrentEmployee.NgaySinh = DateOnly.FromDateTime(dpNgaySinh.SelectedDate.Value);
                else
                    CurrentEmployee.NgaySinh = null;

                // Ngày nhận việc
                if (dpNgayNhanViec.SelectedDate.HasValue)
                    CurrentEmployee.NgayNhanViec = DateOnly.FromDateTime(dpNgayNhanViec.SelectedDate.Value);
                else
                    CurrentEmployee.NgayNhanViec = null;

                // Chức vụ
                if (cbChucVu.SelectedItem is ComboBoxItem item)
                    CurrentEmployee.ChucVu = item.Content.ToString();
                else
                    CurrentEmployee.ChucVu = null;

                // -------------------------------
                // 2) LƯU VÀO DATABASE
                // -------------------------------
                using (var context = new QuanLyVatTuContext())
                {
                    if (CurrentEmployee.MaNhanVien == 0)
                    {
                        context.NhanVien.Add(CurrentEmployee);
                    }
                    else
                    {
                        context.NhanVien.Update(CurrentEmployee);
                    }

                    context.SaveChanges();
                }

                MessageBox.Show("Lưu thành công!", "Thông báo");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi lưu:\n" + ex.Message +
                    "\n\nChi tiết:\n" + ex.InnerException?.Message);
            }
        }
        private bool ValidateEmployee()
        {
            // ===== 1. Kiểm tra rỗng =====
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo");
                txtHoTen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo");
                txtSDT.Focus();
                return false;
            }

            // ===== 1.1 RÀNG BUỘC SỐ ĐIỆN THOẠI (10 SỐ, CHỈ CHỨA SỐ) =====
            string sdt = txtSDT.Text.Trim();

            if (sdt.Length != 10)
            {
                MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số!", "Thông báo");
                txtSDT.Focus();
                return false;
            }

            if (!sdt.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại chỉ được nhập chữ số!", "Thông báo");
                txtSDT.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập username!", "Thông báo");
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo");
                txtPassword.Focus();
                return false;
            }

            if (!dpNgaySinh.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh!", "Thông báo");
                return false;
            }

            if (!dpNgayNhanViec.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày nhận việc!", "Thông báo");
                return false;
            }

            if (cbChucVu.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn chức vụ!", "Thông báo");
                return false;
            }

            // ===== 2. RÀNG BUỘC NGÀY =====
            DateTime ngaySinh = dpNgaySinh.SelectedDate.Value;
            DateTime ngayNhanViec = dpNgayNhanViec.SelectedDate.Value;
            DateTime today = DateTime.Today;

            // 2.1 Ngày nhận việc ±7 ngày
            if (ngayNhanViec < today.AddDays(-7) || ngayNhanViec > today.AddDays(7))
            {
                MessageBox.Show("Ngày nhận việc chỉ được trong khoảng ±7 ngày so với hôm nay!", "Thông báo");
                return false;
            }

            // 2.2 Ngày sinh < ngày nhận việc
            if (ngaySinh >= ngayNhanViec)
            {
                MessageBox.Show("Ngày sinh phải nhỏ hơn ngày nhận việc!", "Thông báo");
                return false;
            }

            // 2.3 Đủ 18 tuổi
            if (ngaySinh.AddYears(18) > ngayNhanViec)
            {
                MessageBox.Show("Nhân viên phải đủ 18 tuổi tại ngày nhận việc!", "Thông báo");
                return false;
            }

            // ===== 3. KIỂM TRA SỐ ĐIỆN THOẠI DUY NHẤT =====
            using (var context = new QuanLyVatTuContext())
            {
                bool isTrungSDT = context.NhanVien.Any(nv =>
                    nv.SoDienThoai == sdt &&
                    nv.MaNhanVien != CurrentEmployee.MaNhanVien);

                if (isTrungSDT)
                {
                    MessageBox.Show("Số điện thoại đã tồn tại. Vui lòng nhập số khác!", "Thông báo");
                    txtSDT.Focus();
                    return false;
                }
            }

            return true;
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


        // Đóng form (nút X)

        private void Button_Click(object sender, RoutedEventArgs e) { this.Close(); }
        private bool IsDataChanged()
        {
            // Thêm mới → không coi là thay đổi
            if (_originalEmployee == null || _originalEmployee.MaNhanVien == 0)
                return false;

            if ((CurrentEmployee.HoTen ?? "").Trim() != (_originalEmployee.HoTen ?? "").Trim())
                return true;

            if ((CurrentEmployee.SoDienThoai ?? "").Trim() != (_originalEmployee.SoDienThoai ?? "").Trim())
                return true;

            if ((CurrentEmployee.Username ?? "").Trim() != (_originalEmployee.Username ?? "").Trim())
                return true;

            if ((CurrentEmployee.Password ?? "").Trim() != (_originalEmployee.Password ?? "").Trim())
                return true;

            if (CurrentEmployee.NgaySinh != _originalEmployee.NgaySinh)
                return true;

            if (CurrentEmployee.NgayNhanViec != _originalEmployee.NgayNhanViec)
                return true;

            if ((CurrentEmployee.ChucVu ?? "") != (_originalEmployee.ChucVu ?? ""))
                return true;

            return false;
        }
        private void RestoreOriginalData()
        {
            if (_originalEmployee == null) return;

            CurrentEmployee.HoTen = _originalEmployee.HoTen;
            CurrentEmployee.SoDienThoai = _originalEmployee.SoDienThoai;
            CurrentEmployee.Username = _originalEmployee.Username;
            CurrentEmployee.Password = _originalEmployee.Password;
            CurrentEmployee.NgaySinh = _originalEmployee.NgaySinh;
            CurrentEmployee.NgayNhanViec = _originalEmployee.NgayNhanViec;
            CurrentEmployee.ChucVu = _originalEmployee.ChucVu;

            // Cập nhật UI
            txtHoTen.Text = CurrentEmployee.HoTen;
            txtSDT.Text = CurrentEmployee.SoDienThoai;
            txtUsername.Text = CurrentEmployee.Username;
            txtPassword.Password = CurrentEmployee.Password;

            dpNgaySinh.SelectedDate = CurrentEmployee.NgaySinh?.ToDateTime(TimeOnly.MinValue);
            dpNgayNhanViec.SelectedDate = CurrentEmployee.NgayNhanViec?.ToDateTime(TimeOnly.MinValue);

            foreach (ComboBoxItem item in cbChucVu.Items)
            {
                if (item.Content.ToString() == CurrentEmployee.ChucVu)
                {
                    cbChucVu.SelectedItem = item;
                    break;
                }
            }
        }


    }
}
