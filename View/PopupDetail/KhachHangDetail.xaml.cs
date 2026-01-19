using System;
using System.Windows;
using System.Windows.Controls;
using PageNavigation.Model;
using PageNavigation.Utilities;

namespace PageNavigation.View.PopupDetail
{
    public partial class KhachHangDetail : Window
    {
        public KhachHangM CurrentCustomer { get; set; }
        private KhachHangM _originalCustomer;


        public KhachHangDetail(KhachHangM customerToEdit = null)
        {
            InitializeComponent();

            if (customerToEdit == null)
            {

                CurrentCustomer = new KhachHangM();
                CurrentCustomer.NgaySinh = DateOnly.FromDateTime(DateTime.Now);
                _originalCustomer = null;
            }
            else
            {
                CurrentCustomer = customerToEdit;
                _originalCustomer = new KhachHangM
                {
                    MaKhachHang = customerToEdit.MaKhachHang,
                    HoVaTen = customerToEdit.HoVaTen,
                    SoDienThoai = customerToEdit.SoDienThoai,
                    DiaChi = customerToEdit.DiaChi,
                    GioiTinh = customerToEdit.GioiTinh,
                    NgaySinh = customerToEdit.NgaySinh
                };
                if (btnSave != null) btnSave.IsEnabled = true;
            }

            this.DataContext = CurrentCustomer;
            CheckValidate();
        }

        private void Form_Changed(object sender, RoutedEventArgs e)
        {
            CheckValidate();
        }

        private void CheckValidate()
        {
            if (txtHoTen == null || txtSDT == null || txtDiaChi == null || cbGioiTinh == null || btnSave == null)
                return;

            bool coTen = !string.IsNullOrWhiteSpace(txtHoTen.Text);
            bool coSDT = !string.IsNullOrWhiteSpace(txtSDT.Text);


            btnSave.IsEnabled = coTen && coSDT;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sdt = txtSDT.Text?.Trim();

                // 1. Kiểm tra đủ 10 số
                if (!IsValidPhoneNumber(sdt))
                {
                    MessageBox.Show(
                        "Số điện thoại phải gồm đúng 10 chữ số và chỉ chứa số!",
                        "Lỗi nhập liệu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    txtSDT.Focus();
                    return;
                }

                using (var context = new QuanLyVatTuContext())
                {
                    // 2. Kiểm tra trùng số điện thoại
                    bool isDuplicatePhone = context.KhachHang.Any(kh =>
                        kh.SoDienThoai == sdt &&
                        kh.MaKhachHang != CurrentCustomer.MaKhachHang
                    );

                    if (isDuplicatePhone)
                    {
                        MessageBox.Show(
                            "Số điện thoại này đã tồn tại. Vui lòng nhập số khác!",
                            "Trùng dữ liệu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        txtSDT.Focus();
                        return;
                    }

                    // 3. Lưu dữ liệu
                    if (CurrentCustomer.MaKhachHang == 0)
                    {
                        context.KhachHang.Add(CurrentCustomer);
                    }
                    else
                    {
                        context.KhachHang.Update(CurrentCustomer);
                    }

                    context.SaveChanges();
                    GlobalEvents.RaiseKhachHangChanged();
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

        private void Button_Click(object sender, RoutedEventArgs e) { this.Close(); }
        private bool IsDataChanged()
        {
            if (_originalCustomer == null || _originalCustomer.MaKhachHang == 0)
                return false;

            if ((txtHoTen.Text ?? "").Trim() != (_originalCustomer.HoVaTen ?? "").Trim())
                return true;

            if ((txtSDT.Text ?? "").Trim() != (_originalCustomer.SoDienThoai ?? "").Trim())
                return true;

            if ((txtDiaChi.Text ?? "").Trim() != (_originalCustomer.DiaChi ?? "").Trim())
                return true;

            // SO SÁNH GIỚI TÍNH ĐÚNG CÁCH
            string gioiTinhUI = null;
            if (cbGioiTinh.SelectedItem is ComboBoxItem gtItem)
                gioiTinhUI = gtItem.Content?.ToString();

            if ((gioiTinhUI ?? "") != (_originalCustomer.GioiTinh ?? ""))
                return true;

            if (CurrentCustomer.NgaySinh != _originalCustomer.NgaySinh)
                return true;

            return false;
        }

        private void RestoreOriginalData()
        {
            if (_originalCustomer == null) return;

            CurrentCustomer.HoVaTen = _originalCustomer.HoVaTen;
            CurrentCustomer.SoDienThoai = _originalCustomer.SoDienThoai;
            CurrentCustomer.DiaChi = _originalCustomer.DiaChi;
            CurrentCustomer.GioiTinh = _originalCustomer.GioiTinh;
            CurrentCustomer.NgaySinh = _originalCustomer.NgaySinh;

            txtHoTen.Text = _originalCustomer.HoVaTen;
            txtSDT.Text = _originalCustomer.SoDienThoai;
            txtDiaChi.Text = _originalCustomer.DiaChi;
            cbGioiTinh.Text = _originalCustomer.GioiTinh;

            CheckValidate();
        }
        private bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            phone = phone.Trim();

            // Chỉ cho phép số và đủ 10 chữ số
            return phone.Length == 10 && phone.All(char.IsDigit);
        }



    }
}