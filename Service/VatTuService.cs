using Microsoft.EntityFrameworkCore;
using PageNavigation.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PageNavigation.Service
{
    public static class VatTuService
    {
        // -------------------------
        // Lấy toàn bộ vật tư
        // -------------------------
        public static List<VatTuM> GetAll()
        {
            using (var db = new QuanLyVatTuContext())
            {
                return db.VatTu.ToList();
            }
        }

        // -------------------------
        // Lưu vật tư (Thêm hoặc Cập nhật)
        // -------------------------
        public static bool Save(VatTuM vt)
        {
            try
            {
                using (var db = new QuanLyVatTuContext())
                {
                    if (vt.MaVatTu == 0)
                    {
                        // Thêm mới
                        db.VatTu.Add(vt);
                    }
                    else
                    {
                        // Cập nhật
                        db.VatTu.Update(vt);
                    }

                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu vật tư: " + ex.Message);
                return false;
            }
        }

        // -------------------------
        // Xóa vật tư
        // -------------------------
        public static bool Delete(int maVatTu)
        {
            try
            {
                using (var db = new QuanLyVatTuContext())
                {
                    var vt = db.VatTu.Find(maVatTu);
                    if (vt == null) return false;

                    db.VatTu.Remove(vt);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa vật tư: " + ex.Message);
                return false;
            }
        }
        public static async Task<List<VatTuM>> GetAllAsync()
        {
            using (var db = new QuanLyVatTuContext())
            {
                return await db.VatTu.ToListAsync();
            }
        }

    }
}
