using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.Users;
using CapstoneProject.Databases.Schemas.System.Employee;
using CapstoneProject.Databases.Schemas.Setting;
using CapstoneProject.Databases.Schemas.System.Ticket;

namespace CapstoneProject.Databases.Schemas.System.Order
{
    [Table("Orders")]
    public partial class Orders : TableHaveIdInt, ITable
    {
        public Orders()
        {
            OrderFoodDetails = new HashSet<OrderFoodDetail>();
            OrderTicketDetails = new HashSet<OrderTicketDetail>();
        }
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// Id khách hàng
        /// </summary>
        [AuditIgnore]
        public int UserId { get; set; }
        /// <summary>
        /// Trạng thái đơn hàng
        /// 10: Chưa thanh toán
        /// 20: Thah toán thất bại
        /// 30: Đã thanh toán
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Nhan vien ban
        /// </summary>
        public int ? EmployeeId { get; set; }
        /// <summary>
        /// Mã chi nhánh
        /// </summary>
        public int? BranchId { get; set; }

        /// <summary>
        /// Mã xuất chiếu
        /// </summary>
        public int? ShowTimeId { get; set; }

        public int? PaymentId { get; set; }

        /// <summary>
        /// Ngày xác nhận
        /// </summary>
        public DateTime? ConfirmAt { set; get; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual User User { set; get; }

        public virtual Employees ? Employee { get; set; }

        public virtual Branch? Branches { set; get; }

        public virtual ShowTime ShowTime { set; get; }

        public virtual Payment? Payments { set; get; }

        public virtual ICollection<OrderFoodDetail> OrderFoodDetails { get; set; }

        public virtual ICollection<OrderTicketDetail> OrderTicketDetails { get; set; }
    }
}

