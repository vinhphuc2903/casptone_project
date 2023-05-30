using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using CapstoneProject.Databases.Schemas.Setting;
using CapstoneProject.Databases.Schemas.System.Orders;

namespace CapstoneProject.Databases.Schemas.System.Food
{
    [Table("Foods")]
    public partial class Foods : TableHaveIdInt, ITable
    {
        public Foods ()
        {
            OrderFoodDetails = new HashSet<OrderFoodDetail>();
        }
        public string NameOption1 { get; set; }

        public string? NameOption2 { get; set; }

        public int Price { get; set; }

        public int? SizeId { get; set; }
        /// <summary>
        /// Type:
        /// - 10: Bắp
        /// - 20: Oshi  -Bimbim
        /// - 30: Nước ngọt
        /// </summary>
        public int Type { get; set; }

        public string? Status { get; set; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual Size Size { get; set; }

        public virtual ICollection<OrderFoodDetail> ? OrderFoodDetails { get; set; }
    }
}

