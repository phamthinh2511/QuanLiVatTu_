using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PageNavigation.ViewModel
{
    public class PhieuNhapVatTuVM : INotifyPropertyChanged
    {
        // Danh sách phiếu nhập để Binding lên ListView
        private ObservableCollection<PhieuNhapVatTuM> _listPhieuNhap;
        public ObservableCollection<PhieuNhapVatTuM> ListPhieuNhap
        {
            get => _listPhieuNhap;
            set { _listPhieuNhap = value; OnPropertyChanged(); }
        }

        public PhieuNhapVatTuVM()
        {
            // Tự động tải dữ liệu khi khởi tạo
            LoadData();
        }

        // --- HÀM LOAD DATA (Sửa lỗi vm.LoadData() của bạn) ---
        public void LoadData()
        {
            try
            {
                using (var db = new QuanLyVatTuContext())
                {
                    // 1. Tải phiếu nhập + Kèm theo Chi tiết + Kèm theo Nhân viên trong chi tiết
                    var data = db.PhieuNhapVatTu
                                 .Include(p => p.CT_PhieuNhapVatTu)
                                 .ThenInclude(ct => ct.MaNhanVienNavigation) // Load thông tin nhân viên
                                 .OrderByDescending(x => x.NgayNhapPhieu)
                                 .ToList();

                    // 2. Điền tên nhân viên vào thuộc tính ảo
                    foreach (var item in data)
                    {
                        // Lấy nhân viên từ dòng chi tiết đầu tiên (nếu có)
                        var chiTietDauTien = item.CT_PhieuNhapVatTu.FirstOrDefault();
                        if (chiTietDauTien != null && chiTietDauTien.MaNhanVienNavigation != null)
                        {
                            item.TenNhanVien = chiTietDauTien.MaNhanVienNavigation.HoTen;
                        }
                        else
                        {
                            item.TenNhanVien = "---";
                        }
                    }

                    ListPhieuNhap = new ObservableCollection<PhieuNhapVatTuM>(data);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}