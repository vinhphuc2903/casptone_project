using System;
namespace CapstoneProject.Areas.ShowTime.Models.Schemas
{
	public class ShowTimeInput
    {
		public ShowTimeInput()
		{
		}
        /// <summary>
        /// Ten xuat chieu
        /// </summary>
        //public string Name { get; set; }
        /// <summary>
        /// Ma xuat chieu
        /// </summary>
        //public string Code { get; set; }
        /// <summary>
        /// Từ ngày
        /// </summary>
        public DateTime DateFrom { get; set; }
		/// <summary>
		/// Đến ngày
		/// </summary>
		public DateTime DateTo { get; set; }
		/// <summary>
		/// Thời gian bắt đầu
		/// </summary>
		public DateTime TimeFrom { get; set; }
		/// <summary>
		/// Thời gian kết thúc
		/// </summary>
		//public DateTime TimeTo { get; set; }
		/// <summary>
		/// Số xuất chiếu
		/// </summary>
		public int CountShow { get; set; }
		/// <summary>
		/// Số giờ nghỉ của xuất chiếu
		/// </summary>
		public int MinOff { get; set; }
		/// <summary>
		/// Film chiếu
		/// </summary>
		public int IdFilm { get; set; }
		/// <summary>
		/// Mã phòng chiếu
		/// </summary>
		public int IdRoom { get; set; }
        public int BranchId { get; set; }
    }
}

