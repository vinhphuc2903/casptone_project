using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Areas.Users.Models.UserModel.Schemas
{
	public class AccountInfo
	{
		public AccountInfo()
		{
			
		}
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        [Required]
        public string Password { get; set; }

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


    }
}

