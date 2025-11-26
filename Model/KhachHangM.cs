using System;
using System.Collections.Generic;

namespace PageNavigation.Model;

public partial class KhachHangM
{
    public int MaKhachHang { get; set; }

    public string HoVaTen { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public virtual ICollection<HoaDonM> HoaDon { get; set; } = new List<HoaDonM>();

    public virtual ICollection<PhieuThuTienM> PhieuThuTien { get; set; } = new List<PhieuThuTienM>();
}
