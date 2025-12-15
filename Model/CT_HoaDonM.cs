using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PageNavigation.Model;

public partial class CT_HoaDonM
{
    public int MaHoaDon { get; set; }

    public int MaVatTu { get; set; }

    public int? MaDonViTinh { get; set; }

    public int SoLuongBan { get; set; }

    public decimal? DonGiaBan { get; set; }

    public decimal? ThanhTien { get; set; }

    public virtual DonViTinhM? MaDonViTinhNavigation { get; set; }

    public virtual HoaDonM MaHoaDonNavigation { get; set; } = null!;

    public virtual VatTuM MaVatTuNavigation { get; set; } = null!;

    [NotMapped] public string TenVatTu { get; set; }
    [NotMapped] public string TenDonViTinh { get; set; }
}
