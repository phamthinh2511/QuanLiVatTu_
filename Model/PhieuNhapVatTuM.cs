using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PageNavigation.Model;

public partial class PhieuNhapVatTuM
{
    public int MaPhieuNhap { get; set; }

    public DateTime? NgayNhapPhieu { get; set; }

    public decimal? TongTien { get; set; }

    [NotMapped]
    public int? MaNhanVien { get; set; }

    [NotMapped]
    public string TenNhanVien { get; set; }

    // Navigation property cũng bỏ hoặc NotMapped luôn
    [NotMapped]
    public virtual NhanVienM NhanVien { get; set; }

    public virtual ICollection<CT_PhieuNhapVatTuM> CT_PhieuNhapVatTu { get; set; } = new List<CT_PhieuNhapVatTuM>();
}
