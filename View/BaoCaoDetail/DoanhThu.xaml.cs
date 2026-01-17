using PageNavigation.Utilities;
using PageNavigation.View.PopupDetail;
using PageNavigation.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PageNavigation.View.BaoCaoDetail.PopupChart;
namespace PageNavigation.View.BaoCaoDetail
{
    /// <summary>
    /// Interaction logic for TongQuan.xaml
    /// </summary>
    public partial class DoanhThu : UserControl
    {
        public DoanhThu()
        {
            InitializeComponent();
            this.DataContext = new PageNavigation.View.BaoCaoDetail.DoanhThuVM();
        }
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private void DoanhThuListView_Click(object sender, RoutedEventArgs e)
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
            ICollectionView dataView = CollectionViewSource.GetDefaultView(DoanhThuListView.ItemsSource);

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
            DoanhThuChart popup = new DoanhThuChart();
            var result = popup.ShowDialog();

            
        }
    }
}
