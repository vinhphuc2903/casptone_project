using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;

namespace CapstoneProject.Databases.Schemas.System.Orders
{
    [Table("OrderDetails")]
    public class OrderDetails : TableHaveIdInt, ITable
    {
		public OrderDetails()
		{
		}
        /// <summary>
        /// Id khách hàng
        /// </summary>
        [AuditIgnore]
        public int UserId { get; set; }
        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        public string Status { get; set; }
    }
}

