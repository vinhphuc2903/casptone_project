using System;
using System.ComponentModel.DataAnnotations;
using FilmData = CapstoneProject.Databases.Schemas.System.Film.Films;

namespace CapstoneProject.Areas.Film.Models.FilmAdminModels.Schemas
{
    public class NewFilmData
    {
        public int? Id { get; set; }
        /// <summary>
        /// Tên loại phim
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Độ dài phim
        /// </summary>
        public int Time { get; set; }
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
        public DateTime? DateEnd { get; set; }
        /// <summary>
		/// Ngày phát hành
		/// </summary>
        public DateTime? DateRelease { get; set; }
        /// <summary>
        /// Ngày tạm hoãn
        /// </summary>
        public DateTime? DatePostpone { get; set; }
        /// <summary>
        /// Ngày bắt đầu lại
        /// </summary>
        public DateTime? DateStartPostpone { get; set; }
        /// <summary>
        /// Ngày gia hạn ( ngày kết thúc )
        /// </summary>
        public DateTime? DateExtend { get; set; }
        /// <summary>
        /// Lý do tạm hoãn
        /// </summary>
        /// <value></value>
        public string? ReasonPostpone { get; set; }
        /// <summary>
        /// Lý do gia hạn
        /// </summary>
        /// <value></value>
        public string? ReasonExtend { get; set; }
        /// <summary>
		/// Vốn đầu tư
		/// </summary>
		/// <value></value>
		public double? Cost { get; set; }
        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        public string? Language { get; set; }
        /// <summary>
        /// Phụ đề
        /// </summary>
        public bool IsSubtittle { get; set; }
        /// <summary>
        /// Danh sách diễn viên
        /// </summary>
        public string? Actor { get; set; }
        /// <summary>
        /// Danh sách đạo diễn
        /// </summary>
        public string? Director { get; set; }
        /// <summary>
        /// Giới thiệu film
        /// </summary>
        public string Introduce { get; set; }
        /// <summary>
        /// Đất nước 
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// Trạng thái phim
        /// - 10: Phim mới
        /// - 20: Đang công chiếu
        /// - 30: Tạm hoãn
        /// - 40: Đã xóa
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Link ảnh backGround
        /// </summary>
        public IFormFile? BackgroundImage { get; set; }

        // <summary>
        /// Link ảnh backGround
        /// </summary>
        public string? BackgroundImageLink { get; set; }

        /// <summary>
        /// Link trailer film Youtobe
        /// </summary>
        public string TrailerLink { get; set; }
        [Required]
        public string ListTypeFilm { get; set; }
        public List<int>? ListTypeFilmData { get; set; }
    }
}

