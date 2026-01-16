using PageNavigation.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PageNavigation.Session;

namespace PageNavigation
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void togglebutton_Checked(object sender, RoutedEventArgs e)
        {
            Pages.Opacity = 0.3;
            Pages.IsEnabled = false;
        }

        private void togglebutton_Unchecked(object sender, RoutedEventArgs e)
        {
            Pages.Opacity = 1;
            Pages.IsEnabled = true;
        }

        private void closebutton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        

    }
}