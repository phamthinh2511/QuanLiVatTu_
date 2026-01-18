using LiveCharts;
using LiveCharts.Wpf;
using PageNavigation.Utilities;
using PageNavigation.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PageNavigation.View.BaoCaoDetail.PopupChart;

namespace PageNavigation.View.BaoCaoDetail
{
    /// <summary>
    /// Interaction logic for ChiTiet.xaml
    /// </summary>
    public partial class TonKho : UserControl
    {
        public TonKho()
        {
            InitializeComponent();
            this.DataContext = new BaoCaoVM();
        }
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private void BaoCaoListView_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked == null || headerClicked.Role == GridViewColumnHeaderRole.Padding) return;
            string propertyName = GridViewSort.GetPropertyName(headerClicked.Column);
            if (string.IsNullOrEmpty(propertyName)) return;

            if (headerClicked != _lastHeaderClicked)
            {
                direction = ListSortDirection.Ascending;
            }
            else
            {
                if (_lastDirection == ListSortDirection.Ascending)
                    direction = ListSortDirection.Descending;
                else
                    direction = ListSortDirection.Ascending;
            }

            Sort(propertyName, direction);

            if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
            {
                _lastHeaderClicked.Column.HeaderTemplate = null;
            }

            if (direction == ListSortDirection.Ascending)
            {
                headerClicked.Column.HeaderTemplate = Resources["ArrowUp"] as DataTemplate;
            }
            else
            {
                headerClicked.Column.HeaderTemplate = Resources["ArrowDown"] as DataTemplate;
            }
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(BaoCaoListView.ItemsSource);

            if (dataView != null)
            {
                dataView.SortDescriptions.Clear();
                SortDescription sd = new SortDescription(sortBy, direction);
                dataView.SortDescriptions.Add(sd);
                dataView.Refresh();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as BaoCaoVM;
            TonKhoChart chart = new TonKhoChart(vm.ListBaoCao, vm.NamBaoCao);
            chart.ShowDialog();
        }
    }
}
