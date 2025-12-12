using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace PageNavigation.Helpers
{
    public static class PasswordBoxHelper
    {
        // 1) BoundPassword (string) - để bind như bình thường (Mode=TwoWay).
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        private static readonly DependencyProperty UpdatingPasswordProperty =
            DependencyProperty.RegisterAttached(
                "UpdatingPassword",
                typeof(bool),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(false));

        public static string GetBoundPassword(DependencyObject dp) =>
            (string)dp.GetValue(BoundPasswordProperty);

        public static void SetBoundPassword(DependencyObject dp, string value) =>
            dp.SetValue(BoundPasswordProperty, value);


        private static bool GetUpdatingPassword(DependencyObject dp) =>
            (bool)dp.GetValue(UpdatingPasswordProperty);

        private static void SetUpdatingPassword(DependencyObject dp, bool value) =>
            dp.SetValue(UpdatingPasswordProperty, value);


        private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is not PasswordBox passwordBox) return;

            // Unsubscribe first to avoid double subscriptions
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            var newPassword = (string?)e.NewValue ?? string.Empty;

            if (!GetUpdatingPassword(passwordBox))
            {
                // update PasswordBox.Password without triggering loop
                passwordBox.Password = newPassword;
            }

            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is not PasswordBox passwordBox) return;

            SetUpdatingPassword(passwordBox, true);
            SetBoundPassword(passwordBox, passwordBox.Password);
            SetUpdatingPassword(passwordBox, false);

            // Also update SecurePassword attachment if used
            SetBoundSecurePassword(passwordBox, passwordBox.SecurePassword);
        }


        // 2) BoundSecurePassword (SecureString) - optional: binds SecureString
        public static readonly DependencyProperty BoundSecurePasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundSecurePassword",
                typeof(SecureString),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(null, OnBoundSecurePasswordChanged));

        public static SecureString GetBoundSecurePassword(DependencyObject dp) =>
            (SecureString)dp.GetValue(BoundSecurePasswordProperty);

        public static void SetBoundSecurePassword(DependencyObject dp, SecureString value) =>
            dp.SetValue(BoundSecurePasswordProperty, value);

        private static void OnBoundSecurePasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            // we intentionally do NOT set PasswordBox.SecurePassword because SecurePassword is readonly
            // this method exists to allow binding from ViewModel -> UI if needed (rare)
        }
    }
}
