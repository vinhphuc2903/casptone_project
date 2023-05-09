using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.Users;

namespace CapstoneProject.Databases.Schemas.System.Orders
{
    [Table("Orders")]
    public partial class Orders : TableHaveIdInt, ITable
    {
        /// <summary>
        /// Id khách hàng
        /// </summary>
        [AuditIgnore]
        public int UserId { get; set; }
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
        public string UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual User User { set; get; }

    }
}

