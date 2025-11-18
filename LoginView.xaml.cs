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

namespace PageNavigation
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();

            var viewModel = this.DataContext as LoginVM;

            if (viewModel != null)
            {
                // 2. ĐĂNG KÝ SỰ KIỆN:
                // Nếu ViewModel phát tín hiệu "LoginSuccess"
                // thì chạy hàm "OnLoginSuccess" của chúng ta
                viewModel.LoginSuccess += OnLoginSuccess;
            }
        }

        private void OnLoginSuccess(object sender, System.EventArgs e)
        {
            // Mở cửa sổ MainWindow
            // (Đảm bảo bạn đã có file MainWindow.xaml trong project View)
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // Đóng cửa sổ Login hiện tại
            this.Close();
        }

        private void MyPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Lấy "bộ não" (ViewModel)
            var viewModel = this.DataContext as LoginVM;

            if (viewModel != null)
            {
                viewModel.Password = MyPasswordBox.Password;
            }
        }

    }
}
