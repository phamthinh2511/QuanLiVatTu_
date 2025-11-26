using System;
using System.Collections.Generic;

namespace PageNavigation.Model;

public partial class BaoCaoM
{
    public int MaVatTu { get; set; }

    public int Thang { get; set; }

    public int Nam { get; set; }

    public int? TonDau { get; set; }

    public int? PhatSinhNhap { get; set; }

    public int? PhatSinhXuat { get; set; }

    public int? TonCuoi { get; set; }

    public virtual VatTuM MaVatTuNavigation { get; set; } = null!;
}
