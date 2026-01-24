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
    /// Interaction logic for LoaiVatTu.xaml
    /// </summary>
    public partial class LoaiVatTu : UserControl
    {
        public LoaiVatTu()
        {
            InitializeComponent();
            DataContext = new LoaiVatTuVM();
        }

        private async void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            //var popup = new LoaiVatTuDetail();

            //if (popup.ShowDialog() == true)
            // {
            // if (DataContext is LoaiVatTuVM vm)
            // {
            //      await vm.LoadDataAsync();
            //   //  }
            // }
            var vm = this.DataContext as LoaiVatTuVM;
            if (vm != null)
            {
                // Tạo mới đối tượng để hứng dữ liệu
                vm.CurrentLoaiVatTu = new LoaiVatTuM();

                var popup = new LoaiVatTuDetail();

                // QUAN TRỌNG: Gán DataContext để popup hiểu biến CurrentLoaiVatTu nằm ở đâu
                popup.DataContext = vm;

                if (popup.ShowDialog() == true)
                {
                    // Code save vào database (nếu cần) hoặc reload lại list
                     await vm.LoadDataAsync(); 
                }
            }

        }

        private async void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as LoaiVatTuVM;
            if (vm == null) return;

            // 1. Lấy item đang chọn
            var selected = VatTuTypeListView.SelectedItem as LoaiVatTuM;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn loại vật tư cần sửa!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Gửi object sang popup để sửa
            var popup = new LoaiVatTuDetail(selected);

            // 3. Nếu người dùng bấm Lưu → reload lại danh sách
            if (popup.ShowDialog() == true)
            {
                await vm.LoadDataAsync();
            }
            else
            {
                // Người dùng ấn Cancel hoặc tắt popup → làm mới UI để tránh bind sai
                await vm.LoadDataAsync();
            }
        }

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as LoaiVatTuVM;
            if (vm == null) return;

            var selected = VatTuTypeListView.SelectedItem as LoaiVatTuM;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn loại vật tư cần xóa!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa loại vật tư:\n\n{selected.TenLoai}?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                using (var context = new QuanLyVatTuContext())
                {
                    // 🔴 1. KIỂM TRA ĐÃ CÓ VẬT TƯ SỬ DỤNG CHƯA
                    bool daSuDung = context.VatTu
                        .Any(vt => vt.MaLoai == selected.MaLoai);

                    if (daSuDung)
                    {
                        MessageBox.Show(
                            "Không thể xóa loại vật tư này vì đã có vật tư sử dụng!",
                            "Không thể xóa",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

                    // 🔴 2. LOAD LẠI ENTITY TRONG CONTEXT HIỆN TẠI
                    var loaiVatTu = context.LoaiVatTu
                        .FirstOrDefault(l => l.MaLoai == selected.MaLoai);

                    if (loaiVatTu != null)
                    {
                        context.LoaiVatTu.Remove(loaiVatTu);
                        context.SaveChanges();
                    }
                }

                await vm.LoadDataAsync();

                MessageBox.Show("Xóa thành công!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa dữ liệu.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }
    }
}
