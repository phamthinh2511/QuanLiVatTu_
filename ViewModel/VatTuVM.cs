using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using PageNavigation.Service;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PageNavigation.ViewModel
{
    public class VatTuVM : Utilities.ViewModelBase
    {
        public LoaiVatTuVM LoaiVatTu { get; set; } = new LoaiVatTuVM();

        
        // --------------------------------------------
        // 1) Danh sách vật tư (ListView)
        // --------------------------------------------
        private ObservableCollection<VatTuM> _DanhSachVatTu;
        public ObservableCollection<VatTuM> DanhSachVatTu
        {
            get => _DanhSachVatTu;
            set
            {
                _DanhSachVatTu = value;
                OnPropertyChanged();
            }
        }

        // --------------------------------------------
        // 2) Vật tư đang chọn (ListView)
        // --------------------------------------------
        private VatTuM _SelectedVatTu;
        public VatTuM SelectedVatTu
        {
            get => _SelectedVatTu;
            set
            {
                _SelectedVatTu = value;
                OnPropertyChanged();
            }
        }

        // --------------------------------------------
        // 3) Model vật tư trong Popup
        // --------------------------------------------
        private VatTuM _VatTu;
        public VatTuM VatTu
        {
            get => _VatTu;
            set
            {
                _VatTu = value;
                OnPropertyChanged();
                LoadLoaiVatTu();
            }
        }

        // --------------------------------------------
        // 4) Danh sách loại vật tư (ComboBox)
        // --------------------------------------------
        private ObservableCollection<LoaiVatTuM> _DanhSachLoaiVatTu;
        public ObservableCollection<LoaiVatTuM> DanhSachLoaiVatTu
        {
            get => _DanhSachLoaiVatTu;
            set
            {
                _DanhSachLoaiVatTu = value;
                OnPropertyChanged();
            }
        }

        // --------------------------------------------
        // 5) Loại vật tư đang chọn (ComboBox)
        // --------------------------------------------
        private LoaiVatTuM _LoaiVatTuSelected;
        public LoaiVatTuM LoaiVatTuSelected
        {
            get => _LoaiVatTuSelected;
            set
            {
                _LoaiVatTuSelected = value;

                if (VatTu != null && value != null)
                    VatTu.MaLoai = value.MaLoai;

                OnPropertyChanged();
            }
        }

        // --------------------------------------------
        // 6) Constructor
        // --------------------------------------------
        public VatTuVM()
        {
            VatTu = new VatTuM();
            LoadLoaiVatTu();
            LoadDataAsync();

            LoaiVatTuService.OnLoaiVatTuChanged += LoadLoaiVatTu;   // ComboBox auto update
            PageNavigation.Utilities.GlobalEvents.OnVatTuChanged += () =>
            {
                LoadDataAsync();
            };
        }

        // --------------------------------------------
        // 7) Load danh sách loại vật tư (ComboBox)
        // --------------------------------------------
        private void LoadLoaiVatTu()
        {
            var data = LoaiVatTuService.GetAll();

            DanhSachLoaiVatTu = new ObservableCollection<LoaiVatTuM>(data);

            if (VatTu != null)
            {
                LoaiVatTuSelected =
                    DanhSachLoaiVatTu.FirstOrDefault(x => x.MaLoai == VatTu.MaLoai);
            }
        }

        // --------------------------------------------
        // 8) Load danh sách vật tư (ListView)
        // --------------------------------------------
        public async Task LoadDataAsync()
        {
            try
            {
                using (var db = new QuanLyVatTuContext())
                {
                    // 2. Lấy dữ liệu mới nhất từ SQL (nhớ Include để lấy cả tên Loại)
                    var data = await db.VatTu
                                       .Include(x => x.MaLoaiNavigation) // Nếu bạn muốn hiện tên Loại
                                       .Include(x => x.MaDonViTinhNavigation)
                                       .ToListAsync();

                    // 3. Cập nhật vào ObservableCollection
                    // Cách an toàn nhất: Tạo mới collection và gán lại
                    DanhSachVatTu = new ObservableCollection<VatTuM>(data);

                    // 4. Báo cho giao diện biết là "Danh sách đã thay đổi rồi, vẽ lại đi!"
                    OnPropertyChanged(nameof(DanhSachVatTu));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // --------------------------------------------
        // 9) Lưu vật tư
        // --------------------------------------------
        public bool SaveVatTu()
        {
            if (!Validate())
                return false;

            var result = VatTuService.Save(VatTu);

            if (result)
                LoadDataAsync(); // reload ListView

            return result;
        }

        // --------------------------------------------
        // 10) Validate
        // --------------------------------------------
        private bool Validate()
        {
            if (VatTu == null) return false;
            if (string.IsNullOrWhiteSpace(VatTu.TenVatTu)) return false;
            if (LoaiVatTuSelected == null) return false;

            return true;
        }

        // --------------------------------------------
        // 11) Reset form khi tạo mới
        // --------------------------------------------
        public void ResetForm()
        {
            VatTu = new VatTuM();

            if (DanhSachLoaiVatTu?.Count > 0)
                LoaiVatTuSelected = DanhSachLoaiVatTu.FirstOrDefault();
        }

        // --------------------------------------------
        // 12) Load khi sửa
        // --------------------------------------------
        public void LoadVatTu(VatTuM vt)
        {
            if (vt == null) return;

            VatTu = vt;

            LoaiVatTuSelected =
                DanhSachLoaiVatTu.FirstOrDefault(x => x.MaLoai == vt.MaLoai);
        }

        // --------------------------------------------
        // 13) Xóa vật tư
        // --------------------------------------------
        public bool DeleteVatTu(VatTuM vt)
        {
            if (vt == null) return false;

            var result = VatTuService.Delete(vt.MaVatTu);

            if (result)
                DanhSachVatTu.Remove(vt);

            return result;
        }
    }
}
