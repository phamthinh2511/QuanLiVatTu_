using System;
using System.Collections.Generic;

namespace PageNavigation.Model;

public partial class PhieuNhapVatTuM
{
    public int MaPhieuNhap { get; set; }

    public DateTime? NgayNhapPhieu { get; set; }

    public decimal? TongTien { get; set; }

    public virtual ICollection<CT_PhieuNhapVatTuM> CT_PhieuNhapVatTu { get; set; } = new List<CT_PhieuNhapVatTuM>();
}
