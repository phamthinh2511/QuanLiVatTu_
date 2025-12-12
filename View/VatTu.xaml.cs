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

namespace PageNavigation.View
{
    /// <summary>
    /// Interaction logic for VatTu.xaml
    /// </summary>
    public partial class VatTu : UserControl
    {
        public VatTu()
        {
            InitializeComponent();
            this.DataContext = new VatTuVM();
        }

        private async void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            // 1. Lấy ViewModel hiện tại của màn hình danh sách
            var vm = DataContext as VatTuVM;
            if (vm == null) return;

            // 2. Mở cửa sổ nhập liệu (VatTuDetail)
            // Truyền null vào ý nói là "Tôi muốn thêm mới, không phải sửa"
            var popup = new VatTuDetail(null);

            // 3. Hiện Popup lên và chờ người dùng thao tác xong
            if (popup.ShowDialog() == true)
            {
                // 4. Nếu bên kia lưu thành công và đóng lại, thì bên này tải lại danh sách
                // (Hàm LoadDataAsync phải có trong VatTuVM nhé)
                await vm.LoadDataAsync();
            }
        }

        private async void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            // Lấy item được chọn trong ListView
            var selected = VatTuListView.SelectedItem as VatTuM;

            if (selected == null)
            {
                return;
            }

            // Gửi đối tượng vào popup để sửa
            VatTuDetail popup = new VatTuDetail(selected);

            var result = popup.ShowDialog();

            // Sau khi đóng popup → Reload lại danh sách
            var vm = this.DataContext as VatTuVM;
            if (vm != null)
            {
                await vm.LoadDataAsync();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            // 1. Lấy ViewModel hiện tại
            var vm = this.DataContext as VatTuVM;
            if (vm == null) return;

            // 2. Lấy vật tư đang được chọn trong ListView
            // (Lưu ý: VatTuListView là tên x:Name bên file XAML)
            var selectedItem = VatTuListView.SelectedItem as VatTuM;

            // 3. Kiểm tra nếu chưa chọn dòng nào thì dừng lại
            if (selectedItem == null)
            {
                // Có thể hiện thông báo nhắc nhở nếu muốn
                // MessageBox.Show("Vui lòng chọn vật tư cần xóa!");
                return;
            }

            // 4. Hiện hộp thoại xác nhận xóa
            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa vật tư: {selectedItem.TenVatTu}?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // 5. Nếu người dùng bấm Yes thì thực hiện xóa
            if (result == MessageBoxResult.Yes)
            {
                // Gọi hàm DeleteVatTu đã viết trong VatTuVM
                bool isDeleted = vm.DeleteVatTu(selectedItem);

                if (isDeleted)
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Không thể xóa vật tư này (có thể do đã có dữ liệu liên quan trong phiếu nhập/xuất).",
                                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
