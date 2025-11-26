using System;
using System.Windows;
using System.Windows.Controls;
using PageNavigation.Model;

namespace PageNavigation.View.PopupDetail
{
    public partial class KhachHangDetail : Window
    {
        public KhachHangM CurrentCustomer { get; set; }

        public KhachHangDetail(KhachHangM customerToEdit = null)
        {
            InitializeComponent();

            if (customerToEdit == null)
            {

                CurrentCustomer = new KhachHangM();
                CurrentCustomer.NgaySinh = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                CurrentCustomer = customerToEdit;
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
                using (var context = new QuanLyVatTuContext())
                {
                    if (CurrentCustomer.MaKhachHang == 0)
                    {

                        context.Khachhangs.Add(CurrentCustomer);
                    }
                    else
                    {
                        context.Khachhangs.Update(CurrentCustomer);
                    }

                    context.SaveChanges();
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

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) { this.DialogResult = false; this.Close(); }
        private void Button_Click(object sender, RoutedEventArgs e) { this.Close(); }
    }
}