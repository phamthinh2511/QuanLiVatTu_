using PageNavigation.Model;
using PageNavigation.View.PopupDetail;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PageNavigation.View.TraCuuDetail
{
    /// <summary>
    /// Interaction logic for KhachHangSearch.xaml
    /// </summary>
    public partial class KhachHangSearch : UserControl
    {
        public KhachHangSearch()
        {
            InitializeComponent();
            this.DataContext = new KhachHangSearchVM();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = CustomerListView.SelectedItem as KhachHangM;

            if (selectedCustomer == null)
            {
                return;
            }

            KhachHangDetail popup = new KhachHangDetail(selectedCustomer);
            var result = popup.ShowDialog();

            var viewModel = this.DataContext as DanhSachKhachHangVM;
            if (viewModel != null)
            {
                viewModel.LoadDataAsync();
            }
        }
    }
}
