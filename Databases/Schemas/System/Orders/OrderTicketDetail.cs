using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.Ticket;

namespace CapstoneProject.Databases.Schemas.System.Orders
{
    [Table("OrderTicketDetail")]
    public partial class OrderTicketDetail : TableHaveIdInt, ITable
    {
		public OrderTicketDetail()
		{
		}
        /// <summary>
        /// Id đơn hàng
        /// </summary>
        [AuditIgnore]
        public int OrderId { get; set; }
        /// <summary>
        /// Id vé đơn hàng
        /// </summary>
        [AuditIgnore]
        public int TicketId { get; set; }
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

        public virtual Tickets Ticket { get; set; }

    }
}

