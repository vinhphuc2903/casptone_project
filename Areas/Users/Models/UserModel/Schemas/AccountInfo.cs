using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Areas.Users.Models.UserModel.Schemas
{
	public class AccountInfo
	{
		public AccountInfo()
		{
			
		}
        public int? Id { get; set; }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Tên người dùng
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [Required]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Required]
        public string Phone { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Địa chỉ cụ thể
        /// </summary>
        public string? Address { get; set; }
        public string? DistrictId { get; set; }
        public string? ProvinceId { get; set; }
        public string? CommuneId { get; set; }

    }
}

