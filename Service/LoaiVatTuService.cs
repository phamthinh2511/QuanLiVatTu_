using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PageNavigation.Service
{
    public static class LoaiVatTuService
    {
        public static event Action OnLoaiVatTuChanged;

        // Lấy danh sách loại vật tư
        public static List<LoaiVatTuM> GetAll()
        {
            using var db = new QuanLyVatTuContext();
            return db.LoaiVatTu.ToList();   // <-- dùng LoaiVatTu
        }

        // Thêm loại vật tư
        public static void Add(LoaiVatTuM model)
        {
            using var db = new QuanLyVatTuContext();
            db.LoaiVatTu.Add(model);         // <-- dùng LoaiVatTu
            db.SaveChanges();
            RaiseChanged();
        }

        // Xoá loại vật tư
        public static void Delete(int maLoai)
        {
            using var db = new QuanLyVatTuContext();
            var item = db.LoaiVatTu.FirstOrDefault(x => x.MaLoai == maLoai);

            if (item != null)
            {
                db.LoaiVatTu.Remove(item);
                db.SaveChanges();
                RaiseChanged();
            }
        }

        private static void RaiseChanged()
        {
            OnLoaiVatTuChanged?.Invoke();
        }
    }
}
