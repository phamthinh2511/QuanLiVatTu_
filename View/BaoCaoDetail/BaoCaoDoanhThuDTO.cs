using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNavigation.View.BaoCaoDetail
{
    public class BaoCaoDoanhThuDTO
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public decimal TongDoanhThu { get; set; }
        public decimal TongVon { get; set; }
        public decimal LoiNhuan => TongDoanhThu - TongVon;
        public int SoDonHang { get; set; }
    }
}
