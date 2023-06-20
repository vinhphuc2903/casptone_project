using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.Users;

namespace CapstoneProject.Databases.Schemas.System.Order
{
    [Table("Payments")]
    public class Payment : TableHaveIdInt, ITable
    {
        public Payment()
		{
		}
        /// <summary>
        /// Tổng giá niêm yết của các order detail
        /// </summary>
        public int? OriginPrice { set; get; }

        /// <summary>
        /// Tổng giá bán của các order detail
        /// </summary>
        public int? Price { set; get; }

        /// <summary>
        /// Tổng PaymentPrice của các order detail
        /// </summary>
        public int? SalePrice { set; get; }

        public int? OrderId { get; set; }

        public int UserId { get; set; }

        public int? DiscountPrice { get; set; }

        public int Total { get; set; }
        /// <summary>
        /// Trạng thái thanh toán
        /// 10 chua thanh toan
        /// 20 thanh toan that bai
        /// 30 da thanh toan
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Ngày thanh toán
        /// </summary>
        public DateTime? PaymentAt { set; get; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual Orders? Orders { get; set; }

        public virtual User User { get; set; }
    }
}

