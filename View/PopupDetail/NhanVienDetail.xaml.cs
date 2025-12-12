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
        public NhanVienDetail(NhanVienM nv = null)
        {
            InitializeComponent();
            if (nv == null)
            {
                CurrentEmployee = new NhanVienM();
                //CurrentEmployee.NgaySinh = DateOnly.FromDateTime(DateTime.Now);
               // CurrentEmployee.NgayNhanViec = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                CurrentEmployee = nv;
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

                MessageBox.Show("Lưu thành công!");
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

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Đóng form (nút X)
       
        private void Button_Click(object sender, RoutedEventArgs e) { this.Close(); }
    }
}
