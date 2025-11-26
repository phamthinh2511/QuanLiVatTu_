using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PageNavigation.Model
{

    public partial class KhachHangM
    {

        [NotMapped]
        public DateTime? NgaySinhHienThi
        {
            get
            {
                return this.NgaySinh?.ToDateTime(TimeOnly.MinValue);
            }
            set
            {

                if (value.HasValue)
                {
                    this.NgaySinh = DateOnly.FromDateTime(value.Value);
                }
                else
                {
                    this.NgaySinh = null;
                }
            }
        }
    }
}