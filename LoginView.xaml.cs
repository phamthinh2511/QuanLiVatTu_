using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using PageNavigation.Session;
using PageNavigation.Utilities;
using PageNavigation.ViewModel;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PageNavigation
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            CheckValidate(); // Kiểm tra nút bấm ngay khi mở
        }

        // 1. Logic Validation (Để bật/tắt nút Login)
        private void Form_Changed(object sender, TextChangedEventArgs e)
        {
            var viewModel = this.DataContext as LoginVM;
            if (viewModel != null) CheckValidate(viewModel);
        }

        private void MyPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as LoginVM;
            if (viewModel != null)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
                CheckValidate(viewModel);
            }
        }

        private void CheckValidate(LoginVM? vm = null)
        {
            if (vm == null) vm = this.DataContext as LoginVM;
            if (vm == null || btnLogin == null) return;

            // Chỉ bật nút khi có cả User và Pass
            bool coUser = !string.IsNullOrWhiteSpace(vm.Username);
            bool coPass = !string.IsNullOrEmpty(vm.Password);

            btnLogin.IsEnabled = coUser && coPass;
        }

        private void closebutton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // 2. Logic Đăng Nhập & Loading (QUAN TRỌNG NHẤT)
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as LoginVM;
            if (viewModel == null) return;

            string inputUser = viewModel.Username;
            string inputPass = viewModel.Password;

            LoadingOverlay.Visibility = Visibility.Visible;
            closebutton.IsEnabled = false;
            btnLogin.IsEnabled = false;
            viewModel.ErrorMessage = "";

            bool loginSuccess = false;
            string errorMessage = "";

            await Task.Run(() =>
            {
                try
                {
                    using (var context = new QuanLyVatTuContext())
                    {
                        var user = context.NhanVien
                            .Include(x => x.Role)
                            .FirstOrDefault(x =>
                                x.Username == inputUser &&
                                x.Password == inputPass);

                        if (user == null)
                            throw new Exception("Sai tên đăng nhập hoặc mật khẩu!");
                        UserSession.MaNhanVien = user.MaNhanVien;
                        UserSession.HoTen = user.HoTen;
                        UserSession.RoleName = user.Role!.RoleName;

                        loginSuccess = true;
                    }
                    Thread.Sleep(800);
                }
                catch (Exception ex)
                {
                    loginSuccess = false;
                    errorMessage = ex.Message;
                }
            });

            if (loginSuccess)
            {
                var mainVM = new NavigationVM();
                MainWindow main = new MainWindow();
                main.DataContext = mainVM;
                main.Show();
                this.Close();
            }
            else
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
                closebutton.IsEnabled = true;
                btnLogin.IsEnabled = true;
                viewModel.ErrorMessage = errorMessage;
            }
        }
    }
}