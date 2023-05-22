using System;
namespace CapstoneProject.Areas.Film.Models.FilmModels.Schemas
{
	public class RoomDetail
	{
        /// <summary>
        /// Tên phòng chiếu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tổng số cột
        /// </summary>
        public int TotalColumn { get; set; }
        /// <summary>
        /// Tổng số hàng
        /// </summary>
        public int TotalRow { get; set; }
        /// <summary>
        /// Tổng số ghế
        /// </summary>
        public int TotalSeat { get; set; }
        /// <summary>
        /// Tên chi nhánh
        /// </summary>
        public string? BranchesName { get; set; }
        /// <summary>
        /// Mã chi nhánh
        /// </summary>
        public string? BranchesCode { get; set; }
        /// <summary>
        /// Địa chỉ chi nhánh
        /// </summary>
        public string? BranchesAddress { get; set; }
        /// <summary>
        /// Tỉnh 
        /// </summary>
        public string? BranchesProvince { get; set; }
        /// <summary>
        /// Quận/ huyện
        /// </summary>
        public string? BranchesDistrict { get; set; }
        /// <summary>
        /// Thị xã
        /// </summary>
        public string? BranchesCommune { get; set; }

    }
}

