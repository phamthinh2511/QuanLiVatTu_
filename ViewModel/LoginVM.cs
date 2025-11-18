using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using PageNavigation.Model;
using PageNavigation.Utilities;

namespace PageNavigation.ViewModel
{
    public class LoginVM : Utilities.ViewModelBase
    {
        // 1. KHAI BÁO EVENT (TÍN HIỆU)
        public event EventHandler? LoginSuccess;

        // 2. KHAI BÁO PROPERTY "Username" (Chữ U hoa)
        private string? _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }

        // 3. KHAI BÁO PROPERTY "Password" (Chữ P hoa)
        private string? _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
                // Báo cho nút bấm tự kiểm tra lại
                (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }


        private string? _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(); // Thông báo cho giao diện cập nhật
            }
        }

        // 4. KHAI BÁO COMMAND

        public ICommand LoginCommand { get; set; }

        // 5. HÀM KHỞI TẠO (Constructor)
        public LoginVM()
        {
            // Gán Command với các hàm
            LoginCommand = new RelayCommand(Algorithm_Login, Condition_CanLogin);
        }

        // 6. HÀM THUẬT TOÁN (Code của bạn)
        private void Algorithm_Login(object obj)
        {
            // Kiểm tra bằng tên Property (U hoa, P hoa)
            if (Username == "admin" && Password == "123")
            {
                // Phát tín hiệu
                LoginSuccess?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // Thêm phần báo lỗi
                ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu!";
            }
        }

        // 7. HÀM ĐIỀU KIỆN
        private bool Condition_CanLogin(object obj)
        {
            // Nút bấm chỉ sáng khi cả 2 đều có chữ
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

    }
}
