using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PageNavigation.Model;
using PageNavigation.Service;
using PageNavigation.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PageNavigation.ViewModel
{
    public class BaoCaoVM : ViewModelBase
    {
		private int _thangBaoCao;

		public int ThangBaoCao
		{
			get { return _thangBaoCao; }
			set 
            {
                if (_thangBaoCao != value)
                {
                    _thangBaoCao = value;
                    OnPropertyChanged();
                    LoadData();
                }
            }
		}
        private int _namBaoCao;

        public int NamBaoCao
        {
            get { return _namBaoCao; }
            set 
            {
                if (_namBaoCao != value)
                {
                    _namBaoCao = value;
                    OnPropertyChanged();
                    LoadData();
                }
            }
        }
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged();  }
        }


        private ObservableCollection<BaoCaoM> _listBaoCao;

		public ObservableCollection<BaoCaoM> ListBaoCao
		{
			get { return _listBaoCao; }
			set { _listBaoCao = value; OnPropertyChanged(); }
		}


        public ObservableCollection<int> ListNam { get; set; } = new ObservableCollection<int>();
        public ICommand ExportExcelCommand { get; set; }
        public BaoCaoVM()
        {
            ListBaoCao = new ObservableCollection<BaoCaoM>();
            ThangBaoCao = DateTime.Now.Month;
            NamBaoCao = DateTime.Now.Year;
            ExportExcelCommand = new RelayCommand(ExportToExcel);
            for (int i = 2000; i <= DateTime.Now.Year + 1; i++)
            {
                ListNam.Add(i);
            }
        }
        private async void LoadData()
        {
            IsBusy = true;

            await Task.Run(() =>
            {
                try
                {
                    BaoCaoService.TinhVaLuuBaoCaoTonKho(ThangBaoCao, NamBaoCao);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
                }
            });

            LoadBaoCaoData();

            IsBusy = false;
        }
        private void LoadBaoCaoData()
        {

            int thang = ThangBaoCao;
            int nam = NamBaoCao;

            using (var context = new QuanLyVatTuContext())
            {
                var data = context.BaoCao
                                  .Include(x => x.MaVatTuNavigation)
                                  .Where(x => x.Thang == thang && x.Nam == nam)
                                  .ToList();

                ListBaoCao = new ObservableCollection<BaoCaoM>(data);
            }
        }
        private void ExportToExcel(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Lưu file Báo Cáo",
                FileName = "BaoCaoTonKho_" + DateTime.Now.ToString("ddMMyyyy_HHmm")
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Báo Cáo");
                        worksheet.Cell(1, 1).Value = "ID";
                        worksheet.Cell(1, 2).Value = "Tên Vật Tư";
                        worksheet.Cell(1, 3).Value = "Tồn Đầu";
                        worksheet.Cell(1, 4).Value = "Tồn Cuối";
                        worksheet.Cell(1, 5).Value = "Phát Sinh Nhập";
                        worksheet.Cell(1, 6).Value = "Phát Sinh Xuất";
                        var headerRange = worksheet.Range("A1:F1");
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                        headerRange.Style.Font.FontColor = XLColor.White;
                        int row = 2;
                        if (ListBaoCao != null)
                        {
                            foreach (var item in ListBaoCao)
                            {
                                worksheet.Cell(row, 1).Value = item.MaVatTu;
                                worksheet.Cell(row, 2).Value = item.MaVatTuNavigation?.TenVatTu;
                                worksheet.Cell(row, 3).Value = item.TonDau;
                                worksheet.Cell(row, 4).Value = item.TonCuoi;
                                worksheet.Cell(row, 5).Value = item.PhatSinhNhap;
                                worksheet.Cell(row, 6).Value = item.PhatSinhXuat;
                                row++;
                            }
                            worksheet.Cell(row, 1).Value = $"Báo cáo tồn kho vào: {ThangBaoCao}/{NamBaoCao}";
                            worksheet.Range(row, 1, row, 4).Merge().Style.Font.Bold = true;
                        }

                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(saveFileDialog.FileName);

                        MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
