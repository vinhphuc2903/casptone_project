using System;
using CapstoneProject.Databases;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Areas.Users.Models.UserModel.Schemas
{
	public class UserInfo
	{
        public int Id { get; set; }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [StringLength(50)]
        public string Username { get; set; }
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
        /// Giới tính
        /// </summary>
        public string Gender { set; get; }

        public List<string> RoleId { get; set; }
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
    }
}

