using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.Food;

namespace CapstoneProject.Databases.Schemas.System.Orders
{
    [Table("OrderFoodDetail")]
    public partial class OrderFoodDetail : TableHaveIdInt, ITable
    {
		public OrderFoodDetail()
		{
        }
        /// <summary>
        /// Id đơn hàng
        /// </summary>
        [AuditIgnore]
        public int OrderId { get; set; }
        /// <summary>
        /// Id đồ ăn
        /// </summary>
        [AuditIgnore]
        public int FoodId { get; set; }
        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        public string Status { get; set; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual Orders Orders { get; set; }

        public virtual Foods Foods { get; set; }

    }
}

