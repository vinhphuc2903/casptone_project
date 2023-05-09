using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;

namespace CapstoneProject.Databases.Schemas.System.Users
{
	[Table("UserPoint")]
	public partial class UserPoint : TableHaveIdInt, ITable
    {
		public UserPoint()
		{

		}
		/// <summary>
		/// Id khách hàng
		/// </summary>
        [AuditIgnore]
        public int UserId { get; set; }
		/// <summary>
		/// Tổng số điểm
		/// </summary>
		public int Point { get; set; }
		/// <summary>
		/// Mã rank tương ứng
		/// </summary>
        public string RankName { set; get; }

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

