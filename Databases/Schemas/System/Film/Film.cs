using System;
namespace CapstoneProject.Databases.Schemas.System.Film
{
	public class Film
	{
		public Film()
		{
		}
        /// <summary>
        /// Tên loại phim
        /// </summary>
        public string Name { get; set; }
		/// <summary>
		/// Độ dài phim
		/// </summary>
		public string Time { get; set; }
		/// <summary>
		/// Độ tuổi giới hạn
		/// </summary>
		public int AgeLimit { get; set; }
		/// <summary>
		/// Ngày bắt đầu chiếu phim
		/// </summary>
		public DateTime DateStart { get; set; }
		/// <summary>
		/// Ngày kết thúc chiếu phim
		/// </summary>
		public DateTime DateEnd { get; set; }
		/// <summary>
		/// Ngôn ngữ
		/// </summary>
		public string Language { get; set; }
		/// <summary>
		/// Phụ đề
		/// </summary>
		public bool IsSubtittle { get; set; }
		/// <summary>
		/// Danh sách diễn viên
		/// </summary>
		public string Actor { get; set; }
		/// <summary>
		/// Đất nước 
		/// </summary>
		public string Country { get; set; }
        /// <summary>
        /// Trạng thái phim
        /// - 10: Phim mới
        /// - 20: Đang công chiếu
        /// - 30: Tạm hoãn
        /// - 40: Đã xóa
        /// </summary>
        public string Status { get; set; }
		public int ? TypeFilmId { get; set; }

    }
}

