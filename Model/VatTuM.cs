using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PageNavigation.Model;

public partial class VatTuM
{
    public int MaVatTu { get; set; }

    public string TenVatTu { get; set; } = null!;

    public int MaLoai { get; set; }

    public string? NhaCungCap { get; set; }

    public string? MoTa { get; set; }

    public int? SoLuongTon { get; set; }

    public virtual ICollection<BaoCaoM> BaoCao { get; set; } = new List<BaoCaoM>();

    public virtual ICollection<CT_HoaDonM> CT_HoaDon { get; set; } = new List<CT_HoaDonM>();

    public virtual ICollection<CT_PhieuNhapVatTuM> CT_PhieuNhapVatTu { get; set; } = new List<CT_PhieuNhapVatTuM>();

    public virtual LoaiVatTuM? MaLoaiNavigation { get; set; }
    public string TenLoai => MaLoaiNavigation?.TenLoai ?? "(Không có)";

    [NotMapped]
    public string TenDonViTinh { get; set; }

}
