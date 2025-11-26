using System;
using System.Collections.Generic;

namespace PageNavigation.Model;

public partial class LoaiVatTuM
{
    public int MaLoai { get; set; }

    public string TenLoai { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<VatTuM> Vattus { get; set; } = new List<VatTuM>();
}
