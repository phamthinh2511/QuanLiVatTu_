using System;
using System.Collections.Generic;

namespace PageNavigation.Model;

public partial class DonViTinhM
{
    public int MaDonViTinh { get; set; }

    public string TenDonViTinh { get; set; } = null!;

    public virtual ICollection<CT_HoaDonM>  CT_HoaDon { get; set; } = new List<CT_HoaDonM>();

    public virtual ICollection<CT_PhieuNhapVatTuM> CT_PhieuNhapVatTu { get; set; } = new List<CT_PhieuNhapVatTuM>();
   
    public virtual ICollection<VatTuM> VatTu { get; set; } = new List<VatTuM>();
}
