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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PageNavigation.View.PopupDetail;
using PageNavigation.ViewModel;

namespace PageNavigation.View
{
    /// <summary>
    /// Interaction logic for KhachHang.xaml
    /// </summary>
    public partial class KhachHang : UserControl
    {
        public KhachHang()
        {
            InitializeComponent();
        }

        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedCustomer = CustomerListView.SelectedItem as KhachHangVM;
            if (selectedCustomer != null)
            {
                KhachHangDetail detailWindow = new KhachHangDetail(selectedCustomer);
                detailWindow.ShowDialog();
            }
        }
    } 
}
