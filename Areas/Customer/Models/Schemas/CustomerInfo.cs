using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Areas.Customer.Models.Schemas
{
	public class CustomerInfo
	{
        /// <summary>
        /// Id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Tên dang nhap
        /// </summary>
        public string? Username { get; set; }
        /// <summary>
        /// Mat khau
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// Quyen
        /// </summary>
        public string? Role { get; set; }
        /// <summary>
        /// Rank
        /// </summary>
        public string? Rank { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string? EmployeeCode { get; set; }

        public int? Point { get; set; }
        /// <summary>
        /// Điện thoại
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
		/// Ngày sinh
		/// </summary>
		public DateTime? DateStart { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// Địa chỉ cụ thể
        /// </summary>
        public string? Address { get; set; }
        [StringLength(3)]
        public string? DistrictId { get; set; }

        public string? ProvinceId { get; set; }
        [StringLength(5)]
        public string? CommuneId { get; set; }
    }
}

