using System;
namespace CapstoneProject.Areas.CinemaRoom.Models.Schemas
{
	public class CinemaRoomDataInput
	{
		public CinemaRoomDataInput()
		{
		}
        public int? Id { get; set; }
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

        public int? BranchId { get; set; }

        public string? BranchName { get; set; }
    }
}

