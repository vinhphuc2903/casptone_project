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
        public string Name { get; set; }
        /// <summary>
        /// Ma xuat chieu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Từ ngày
        /// </summary>
        public DateTime DateShow { get; set; }
		/// <summary>
		/// Thời gian bắt đầu
		/// </summary>
		public DateTime TimeFrom { get; set; }
		/// <summary>
		/// Thời gian kết thúc
		/// </summary>
		public DateTime TimeTo { get; set; }
		/// <summary>
		/// Film chiếu
		/// </summary>
		public int IdFilm { get; set; }
		/// <summary>
		/// Mã phòng chiếu
		/// </summary>
		public int IdRoom { get; set; }
	}
}

