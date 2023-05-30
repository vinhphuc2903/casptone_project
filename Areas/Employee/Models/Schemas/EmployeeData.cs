using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Areas.Employee.Models.Schemas
{
	public class EmployeeData
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
        /// Mã nhân viên
        /// </summary>
        public string? EmployeeCode { get; set; }
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
        public string? DistrictName { get; set; }

        public string? ProvinceId { get; set; }
        public string? ProvinceName { get; set; }

        [StringLength(5)]
        public string? CommuneId { get; set; }

        public string? CommuneName { get; set; }

        public int? BranchId { get; set; }

		public string? BranchName { set; get; }

        public int? PositionId { set; get; }

        public string? PositionName { set; get; }
    }
}

