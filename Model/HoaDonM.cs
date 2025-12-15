using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PageNavigation.Model;

public partial class HoaDonM
{
    public int MaHoaDon { get; set; }

    public int? MaKhachHang { get; set; }

    public int? MaNhanVien { get; set; }

    public DateTime? NgayLapHoaDon { get; set; }

    public decimal? TongTien { get; set; }

    public virtual ICollection<CT_HoaDonM> CT_HoaDon { get; set; } = new List<CT_HoaDonM>();

    public virtual KhachHangM? MaKhachHangNavigation { get; set; }

    public virtual NhanVienM? MaNhanVienNavigation { get; set; }

    [NotMapped] public string TenNhanVien { get; set; }
    [NotMapped] public string TenKhachHang { get; set; }
}
