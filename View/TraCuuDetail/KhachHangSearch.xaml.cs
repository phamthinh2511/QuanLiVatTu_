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
        private List<KhachHangM> _allKhachHang;
        public KhachHangSearch()
        {
            InitializeComponent();
            LoadData();
        }
        // ================== LOAD DATA ==================
        private void LoadData()
        {
            using (var db = new QuanLyVatTuContext())
            {
                _allKhachHang = db.KhachHang.ToList();
                CustomerListView.ItemsSource = _allKhachHang;
            }
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

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string hoTen = txtHoTen.Text?.Trim().ToLower();
            string soDienThoai = txtSoDienThoai.Text?.Trim().ToLower();

            var ketQua = _allKhachHang.Where(kh =>
                (string.IsNullOrEmpty(hoTen) ||
                 kh.HoVaTen.ToLower().Contains(hoTen)) &&

                (string.IsNullOrEmpty(soDienThoai) ||
                 kh.SoDienThoai.ToLower().Contains(soDienThoai))
            ).ToList();

            CustomerListView.ItemsSource = ketQua;
        }

        private void BtnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            txtHoTen.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty;

            CustomerListView.ItemsSource = _allKhachHang;
        }
    }
}
