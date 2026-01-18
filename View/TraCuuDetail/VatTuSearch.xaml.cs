using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using PageNavigation.View.PopupDetail;
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
using System.Collections.Generic;
using System.Linq;


namespace PageNavigation.View.TraCuuDetail
{
    /// <summary>
    /// Interaction logic for VatTuSearch.xaml
    /// </summary>
    public partial class VatTuSearch : UserControl
    {
        private List<VatTuM> _allVatTu;
        public VatTuSearch()
        {
            InitializeComponent();
            LoadData();
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
        private void LoadData()
        {
            using (var db = new QuanLyVatTuContext())
            {
                _allVatTu = db.VatTu
                    .Include(vt => vt.MaLoaiNavigation)
                    .ToList();

                VatTuListView.ItemsSource = _allVatTu;
            }
        }
        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string tenVatTu = txtHoTen.Text?.Trim().ToLower();
            string tenLoai = txtSoDienThoai.Text?.Trim().ToLower();

            var ketQua = _allVatTu.Where(vt =>
                (string.IsNullOrEmpty(tenVatTu) ||
                 vt.TenVatTu.ToLower().Contains(tenVatTu)) &&

                (string.IsNullOrEmpty(tenLoai) ||
                 vt.MaLoaiNavigation.TenLoai.ToLower().Contains(tenLoai))
            ).ToList();

            VatTuListView.ItemsSource = ketQua;

        }

        private void BtnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            txtHoTen.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty;

            VatTuListView.ItemsSource = _allVatTu;
        }
    }
}
