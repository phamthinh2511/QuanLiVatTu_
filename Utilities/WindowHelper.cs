using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PageNavigation.Utilities
{

    public static class WindowHelper
    {
        // Khai báo Attached Property có tên là "EnableDrag"
        public static readonly DependencyProperty EnableDragProperty =
            DependencyProperty.RegisterAttached(
                "EnableDrag",
                typeof(bool),
                typeof(WindowHelper),
                new PropertyMetadata(false, OnEnableDragChanged));

        // Getter và Setter cho property này (bắt buộc phải có trong WPF)
        public static bool GetEnableDrag(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableDragProperty);
        }

        public static void SetEnableDrag(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableDragProperty, value);
        }

        // Hàm xử lý khi giá trị property thay đổi
        private static void OnEnableDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Kiểm tra xem đối tượng áp dụng có phải là Window không
            if (d is Window window)
            {
                if ((bool)e.NewValue)
                {
                    // Nếu EnableDrag = true -> Gắn sự kiện kéo chuột
                    window.MouseDown += Window_MouseDown;
                }
                else
                {
                    // Nếu EnableDrag = false -> Gỡ bỏ sự kiện
                    window.MouseDown -= Window_MouseDown;
                }
            }
        }

        // Hàm xử lý logic kéo cửa sổ
        private static void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var window = sender as Window;
                window?.DragMove();
            }
        }
    }

}
