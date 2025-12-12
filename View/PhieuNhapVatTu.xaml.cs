using PageNavigation.Service;
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

namespace PageNavigation.View
{
    /// <summary>
    /// Interaction logic for PhieuNhapVatTu.xaml
    /// </summary>
    public partial class PhieuNhapVatTu : UserControl
    {
        public PhieuNhapVatTu()
        {
            InitializeComponent();
            //PhieuNhapVatTuService.OnPhieuNhapChanged = () =>
          //  {
           //     var vm = this.DataContext as PhieuNhapVatTuVM;
          //      if (vm != null)
           //     {
           //         _ = vm.LoadDataAsync(); // reload đúng cách
           //     }
          //  };

        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            // Mở popup tạo mới
            PhieuNhapVatTuDetail popup = new PhieuNhapVatTuDetail();

            // Nếu người dùng bấm Lưu trong popup
            if (popup.ShowDialog() == true)
            {
                var viewModel = this.DataContext as PhieuNhapVatTuVM;

                if (viewModel != null)
                {
                    viewModel.LoadDataAsync();
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
