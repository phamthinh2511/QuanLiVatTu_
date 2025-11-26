using System;
using System.Collections.Generic;

namespace PageNavigation.Model;

public partial class PhieuThuTienM
{
    public int MaPhieuThu { get; set; }

    public int? MaKhachHang { get; set; }

    public int? MaNhanVien { get; set; }

    public decimal? SoTienThu { get; set; }

    public DateTime? NgayThuTien { get; set; }

    public virtual KhachHangM? MaKhachHangNavigation { get; set; }

    public virtual NhanVienM? MaNhanVienNavigation { get; set; }
}
