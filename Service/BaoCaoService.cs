using System;
using System.Linq;
using System.Collections.Generic;
using PageNavigation.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PageNavigation.View.BaoCaoDetail;

namespace PageNavigation.Service
{
    public class BaoCaoService
    {
        public class DoanhThuService
        {
            public List<BaoCaoDoanhThuDTO> GetBaoCaoDoanhThuTheoNam(int nam)
            {
                if (nam < 2000) return new List<BaoCaoDoanhThuDTO>();

                using (var db = new QuanLyVatTuContext())
                {
                    var resultList = new List<BaoCaoDoanhThuDTO>();
                    for (int i = 1; i <= 12; i++)
                    {
                        resultList.Add(new BaoCaoDoanhThuDTO { Thang = i, Nam = nam });
                    }

                    var listBanHang = db.CT_HoaDon
                                    .Where(x => x.MaHoaDonNavigation.NgayLapHoaDon.HasValue
                                             && x.MaHoaDonNavigation.NgayLapHoaDon.Value.Year == nam)
                                    .Select(x => new
                                    {
                                        Thang = x.MaHoaDonNavigation.NgayLapHoaDon.Value.Month,
                                        MaVatTu = x.MaVatTu,
                                        SoLuongBan = x.SoLuongBan,
                                        ThanhTienBan = x.ThanhTien
                                    })
                                    .ToList();

                    if (!listBanHang.Any()) return resultList;

                    var listGiaVon = db.CT_PhieuNhapVatTu
                        .GroupBy(x => x.MaVatTu)
                        .Select(g => new
                        {
                            MaVatTu = g.Key,
                            GiaNhapTB = g.Sum(x => (decimal?)x.SoLuong) > 0
                                        ? g.Sum(x => x.ThanhTien) / g.Sum(x => (decimal?)x.SoLuong)
                                        : 0
                        })
                        .ToDictionary(x => x.MaVatTu, x => x.GiaNhapTB);

                    foreach (var item in listBanHang)
                    {
                        var reportItem = resultList.First(r => r.Thang == item.Thang);

                        reportItem.TongDoanhThu += item.ThanhTienBan ?? 0;

                        decimal giaVonDonVi = 0;
                        if (listGiaVon.ContainsKey(item.MaVatTu))
                        {
                            giaVonDonVi = listGiaVon[item.MaVatTu] ?? 0;
                        }
                        decimal tongVonItem = (decimal)item.SoLuongBan * giaVonDonVi;
                        reportItem.TongVon += tongVonItem;
                    }
                    var countHoaDon = db.HoaDon
                                        .Where(x => x.NgayLapHoaDon.HasValue && x.NgayLapHoaDon.Value.Year == nam)
                                        .GroupBy(x => x.NgayLapHoaDon.Value.Month)
                                        .Select(g => new { Thang = g.Key, Count = g.Count() })
                                        .ToList();
                     
                    foreach (var c in countHoaDon)
                    {
                        resultList.First(x => x.Thang == c.Thang).SoDonHang = c.Count;
                    }

                    return resultList;
                }
            }
        }
        public static void TinhVaLuuBaoCaoTonKho(int thang, int nam)
        {
            if (nam < 2000 || thang < 1 || thang > 12) return;

            using (var db = new QuanLyVatTuContext())
            {
                DateTime startDate = new DateTime(nam, thang, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                var listVatTu = db.VatTu.ToList();

                var existingReports = db.BaoCao
                                        .Where(x => x.Thang == thang && x.Nam == nam)
                                        .ToList();

                int thangTruoc = thang == 1 ? 12 : thang - 1;
                int namTruoc = thang == 1 ? nam - 1 : nam;
                var listBaoCaoThangTruoc = db.BaoCao
                                             .Where(x => x.Thang == thangTruoc && x.Nam == namTruoc)
                                             .ToList();

                foreach (var vt in listVatTu)
                {
                    int tongNhap = db.CT_PhieuNhapVatTu
                        .Where(x => x.MaVatTu == vt.MaVatTu
                                    && x.MaPhieuNhapNavigation.NgayNhapPhieu >= startDate
                                    && x.MaPhieuNhapNavigation.NgayNhapPhieu <= endDate)
                        .Sum(x => (int?)x.SoLuong) ?? 0;

                    int tongXuat = db.CT_HoaDon
                        .Where(x => x.MaVatTu == vt.MaVatTu
                                    && x.MaHoaDonNavigation.NgayLapHoaDon >= startDate
                                    && x.MaHoaDonNavigation.NgayLapHoaDon <= endDate)
                        .Sum(x => (int?)x.SoLuongBan) ?? 0;
                    var baoCaoThangTruoc = listBaoCaoThangTruoc
                                            .FirstOrDefault(x => x.MaVatTu == vt.MaVatTu);

                    int tonDau = baoCaoThangTruoc?.TonCuoi ?? 0;
                    int tonCuoi = tonDau + tongNhap - tongXuat;
                    var itemBaoCao = existingReports.FirstOrDefault(x => x.MaVatTu == vt.MaVatTu);

                    if (itemBaoCao != null)
                    {
                        itemBaoCao.TonDau = tonDau;
                        itemBaoCao.PhatSinhNhap = tongNhap;
                        itemBaoCao.PhatSinhXuat = tongXuat;
                        itemBaoCao.TonCuoi = tonCuoi;
                    }
                    else
                    {
                        var newItem = new BaoCaoM
                        {
                            Thang = thang,
                            Nam = nam,
                            MaVatTu = vt.MaVatTu,
                            TonDau = tonDau,
                            PhatSinhNhap = tongNhap,
                            PhatSinhXuat = tongXuat,
                            TonCuoi = tonCuoi
                        };
                        db.BaoCao.Add(newItem);
                    }
                }
                db.SaveChanges();
            }
        }
    }
}