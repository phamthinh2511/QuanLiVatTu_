using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNavigation.Service
{
    public static class KhoService
    {
        public static void RebuildTonKho()
        {
            using (var db = new QuanLyVatTuContext())
            {
                var vatTus = db.VatTu.ToList();

                foreach (var vt in vatTus)
                {
                    int tongNhap = db.CT_PhieuNhapVatTu
                        .Where(x => x.MaVatTu == vt.MaVatTu)
                        .Sum(x => (int?)x.SoLuong) ?? 0;

                    int tongBan = db.CT_HoaDon
                        .Where(x => x.MaVatTu == vt.MaVatTu)
                        .Sum(x => (int?)x.SoLuongBan) ?? 0;

                    vt.SoLuongTon = tongNhap - tongBan;
                }

                db.SaveChanges(); // 🔥 DÒNG QUYẾT ĐỊNH
            }
        }
    }

}
