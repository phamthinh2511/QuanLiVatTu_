using System;
using System.Collections.Generic;

namespace PageNavigation.Model;

public partial class NhanVienM
{
    public int MaNhanVien { get; set; }

    public string HoTen { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    public string? SoDienThoai { get; set; }

    public string? ChucVu { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public DateOnly? NgayNhanViec { get; set; }
    public int? RoleID { get; set; }
    public virtual RoleM? Role { get; set; } 

    public virtual ICollection<CT_PhieuNhapVatTuM> CT_PhieuNhapVatTu { get; set; } = new List<CT_PhieuNhapVatTuM>();

    public virtual ICollection<HoaDonM> HoaDon { get; set; } = new List<HoaDonM>();

    public virtual ICollection<PhieuThuTienM> PhieuThuTien { get; set; } = new List<PhieuThuTienM>();
}
