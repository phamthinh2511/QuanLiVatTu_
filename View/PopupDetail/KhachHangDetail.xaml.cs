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
using PageNavigation.ViewModel;

namespace PageNavigation.View.PopupDetail
{
    /// <summary>
    /// Interaction logic for KhachHangDetail.xaml
    /// </summary>
    public partial class KhachHangDetail : Window
    {
        public KhachHangDetail(KhachHangVM customer)
        {
            InitializeComponent();
            this.DataContext = customer;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
