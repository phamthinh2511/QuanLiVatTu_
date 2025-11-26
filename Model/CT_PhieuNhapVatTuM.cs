using System;
using System.Collections.Generic;

namespace PageNavigation.Model;

public partial class CT_PhieuNhapVatTuM
{
    public int MaPhieuNhap { get; set; }

    public int MaVatTu { get; set; }

    public int? MaNhanVien { get; set; }

    public int? MaDonViTinh { get; set; }

    public int SoLuong { get; set; }

    public decimal? DonGiaNhap { get; set; }

    public decimal? ThanhTien { get; set; }

    public decimal? DonGiaBan { get; set; }

    public virtual DonViTinhM? MaDonViTinhNavigation { get; set; }

    public virtual NhanVienM? MaNhanVienNavigation { get; set; }

    public virtual PhieuNhapVatTuM MaPhieuNhapNavigation { get; set; } = null!;

    public virtual VatTuM MaVatTuNavigation { get; set; } = null!;
}
