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
using PageNavigation.View.PopupDetail;

namespace PageNavigation.View.TraCuuDetail
{
    /// <summary>
    /// Interaction logic for VatTuSearch.xaml
    /// </summary>
    public partial class VatTuSearch : UserControl
    {
        public VatTuSearch()
        {
            InitializeComponent();
            this.DataContext = new VatTuSearchVM();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = VatTuListView.SelectedItem as VatTuM;
            if (selectedProduct == null) return;
            VatTuDetail popup = new VatTuDetail(selectedProduct);
            popup.ShowDialog();
            if (this.DataContext is VatTuVM viewModel)
            {
                await viewModel.LoadDataAsync();
            }
        }
    }
}
