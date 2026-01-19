using ClosedXML.Excel;
using Microsoft.Win32;
using PageNavigation.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows;
using System.Windows.Input;
using static PageNavigation.Service.BaoCaoService;

namespace PageNavigation.View.BaoCaoDetail
{
        public class DoanhThuVM : ViewModelBase
        {
            private DoanhThuService _service;

            private ObservableCollection<int> _listNam;
            public ObservableCollection<int> ListNam
            {
                get => _listNam;
                set { _listNam = value; OnPropertyChanged(); }
            }

            private int _namBaoCao;
            public int NamBaoCao
            {
                get => _namBaoCao;
                set
                {
                    _namBaoCao = value;
                    OnPropertyChanged();

                    if (_namBaoCao >= 2000)
                    {
                        LoadDoanhThu();
                    }
                }
            }

            private ObservableCollection<BaoCaoDoanhThuDTO> _listDoanhThu;
            public ObservableCollection<BaoCaoDoanhThuDTO> ListDoanhThu
            {
                get => _listDoanhThu;
                set { _listDoanhThu = value; OnPropertyChanged(); }
            }
        public ICommand ExportExcelCommand { get; set; }
        public DoanhThuVM()
            {
                _service = new DoanhThuService();
                ListNam = new ObservableCollection<int>();
            ExportExcelCommand = new RelayCommand(ExportToExcel);
            int currentYear = DateTime.Now.Year;
                for (int i = 2000; i <= 2040; i++)
                {
                    ListNam.Add(i);
                }

                NamBaoCao = currentYear;
            }

            private async void LoadDoanhThu()
            {

                await Task.Run(() =>
                {

                    var data = _service.GetBaoCaoDoanhThuTheoNam(NamBaoCao);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ListDoanhThu = new ObservableCollection<BaoCaoDoanhThuDTO>(data);
                    });
                });
            }
        private void ExportToExcel(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Lưu file Báo Cáo",
                FileName = "BaoCao_" + DateTime.Now.ToString("ddMMyyyy_HHmm")
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Báo Cáo");
                        worksheet.Cell(1, 1).Value = "Tháng";
                        worksheet.Cell(1, 2).Value = "Doanh Thu";
                        worksheet.Cell(1, 3).Value = "Tổng Vốn";
                        worksheet.Cell(1, 4).Value = "Lợi Nhuận";
                        worksheet.Cell(1, 5).Value = "Số đơn hàng";
                        var headerRange = worksheet.Range("A1:E1");
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                        headerRange.Style.Font.FontColor = XLColor.White;
                        int row = 2;
                        if (ListDoanhThu != null)
                        {
                            foreach (var item in ListDoanhThu)
                            {
                                worksheet.Cell(row, 1).Value = item.Thang;   
                                worksheet.Cell(row, 2).Value = item.TongDoanhThu;
                                worksheet.Cell(row, 3).Value = item.TongVon;
                                worksheet.Cell(row, 4).Value = item.LoiNhuan;
                                worksheet.Cell(row, 5).Value = item.SoDonHang;
                                row++;
                            }
                            worksheet.Cell(row, 1).Value = $"Báo cáo doanh thu vào: {NamBaoCao}";
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


