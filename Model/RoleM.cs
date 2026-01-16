using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PageNavigation.Model
{
    public partial class RoleM
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<NhanVienM> NhanVien { get; set; } = new List<NhanVienM>();
    }
}
