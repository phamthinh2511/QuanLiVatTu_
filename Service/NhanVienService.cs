using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PageNavigation.Service
{
    public static class NhanVienService
    {
        public static event Action OnNhanVienChanged;

        // Lấy danh sách nhân viên
        public static List<NhanVienM> GetAll()
        {
            using var db = new QuanLyVatTuContext();
            return db.NhanVien.ToList();   // <-- bảng NhanVien trong DB
        }

        // Thêm nhân viên
        public static void Add(NhanVienM model)
        {
            using var db = new QuanLyVatTuContext();
            db.NhanVien.Add(model);
            db.SaveChanges();
            RaiseChanged();
        }

        // Xoá nhân viên
        public static void Delete(int maNhanVien)
        {
            using var db = new QuanLyVatTuContext();
            var nv = db.NhanVien.FirstOrDefault(x => x.MaNhanVien == maNhanVien);

            if (nv != null)
            {
                db.NhanVien.Remove(nv);
                db.SaveChanges();
                RaiseChanged();
            }
        }

        private static void RaiseChanged()
        {
            OnNhanVienChanged?.Invoke();
        }
    }
}
