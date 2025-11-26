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

        private void CheckValidate(LoginVM vm = null)
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

            // A. HIỆN LOADING
            if (LoadingOverlay != null) LoadingOverlay.Visibility = Visibility.Visible;
            closebutton.IsEnabled = false;
            btnLogin.IsEnabled = false;

            NavigationVM mainVM = null;
            string errorMessage = "";

            // B. CHẠY NGẦM
            await Task.Run(() =>
            {
                try
                {
                    // C. KIỂM TRA TÀI KHOẢN

                    // Trường hợp 1: Admin mặc định (Vào thẳng)
                    if (inputUser == "admin" && inputPass == "123")
                    {
                        mainVM = new NavigationVM(); // Tải dữ liệu luôn
                    }
                    // Trường hợp 2: Nhân viên (Check SQL)
                    else
                    {
                        using (var context = new PageNavigation.Model.QuanLyVatTuContext())
                        {
                            var user = context.NhanVien.FirstOrDefault(x => x.Username == inputUser && x.Password == inputPass);

                            if (user == null)
                            {
                                throw new Exception("Sai tên đăng nhập hoặc mật khẩu!");
                            }

                            // Đúng nhân viên -> Tải dữ liệu
                            mainVM = new NavigationVM();
                        }
                    }

                    // Ngủ xíu cho hiệu ứng đẹp
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    mainVM = null;
                    errorMessage = ex.Message;
                }
            });

            // D. XỬ LÝ KẾT QUẢ
            if (mainVM != null)
            {
                // Thành công -> Mở Main
                MainWindow main = new MainWindow();
                main.DataContext = mainVM;
                main.Show();
                this.Close();
            }
            else
            {
                // Thất bại -> Tắt Loading, Báo lỗi
                if (LoadingOverlay != null) LoadingOverlay.Visibility = Visibility.Collapsed;
                closebutton.IsEnabled = true;
                btnLogin.IsEnabled = true;

                viewModel.ErrorMessage = errorMessage;

            }
        }
    }
}