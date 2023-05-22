using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases.Schemas.System.Employee;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.Setting;

namespace CapstoneProject.Databases.Schemas.System.Users
{
    [Table("Users")]
    public partial class User : TableHaveIdInt, ITable
    {
        public User()
        {
            Tokens = new HashSet<UserToken>();
            Roles = new HashSet<UserRole>();
        }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [StringLength(50)]
        public string Username { get; set; }
        /// <summary>
        /// Mật khẩu
        /// </summary>
        [AuditIgnore]
        [StringLength(50)]
        public string Password { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [StringLength(255)]
        public string Email { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Phone { get; set; }
        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { set; get; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [AuditIgnore]
        public int? EmployeeId { set; get; }

        /// <summary>
        /// Mã điểm khách hàng
        /// </summary>
        [AuditIgnore]
        public int? UserPointId { set; get; }

        /// <summary>
        /// Giới tính:
        /// M: Nam
        /// F: Nữ
        /// O: Giới tính khác
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// Thời gian login gần nhất
        /// </summary>
        public DateTimeOffset? LastLogin { get; set; }
        /// <summary>
        /// Thời gian logout gần nhất
        /// </summary>
        public DateTimeOffset? LastLogout { get; set; }

        /// <summary>
        /// Địa chỉ cụ thể
        /// </summary>
        public string? Address { get; set; }
        [StringLength(3)]
        public string? DistrictId { get; set; }

        public string? ProvinceId { get; set; }
        [StringLength(5)]
        public string? CommuneId { get; set; }

        [AuditIgnore]
        [StringLength(10)]
        public string FirstSecurityString { get; set; }

        [AuditIgnore]
        [StringLength(10)]
        public string LastSecurityString { get; set; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual Employees Employees { set; get; }

        public virtual UserPoint UserPoint { set; get; }

        public virtual Districts Districts { set; get; }

        public virtual Communes Communes { set; get; }

        public virtual Provinces Provinces { set; get; }


        public virtual ICollection<UserToken> Tokens { set; get; }

        public virtual ICollection<UserRole> Roles { set; get; }

    }
}

