using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.Ticket;

namespace CapstoneProject.Databases.Schemas.System.Film
{
	[Table("Films")]
	public partial class Films : TableHaveIdInt, ITable
	{
		public Films()
		{
			ShowTime = new HashSet<ShowTime>();
            TypeFilmDetail = new HashSet<TypeFilmDetail>();
            //Ticket = new HashSet<Tickets>();
        }
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
		public string BackgroundImage { get; set; }

		/// <summary>
		/// Link trailer film Youtobe
		/// </summary>
		public string TrailerLink { get; set; }

		public DateTimeOffset CreatedAt { set; get; }

		public int CreatedBy { set; get; }

		[StringLength(50)]
		public string? CreatedIp { set; get; }

		public DateTimeOffset? UpdatedAt { set; get; }

		public int? UpdatedBy { set; get; }

		[StringLength(50)]
		public string? UpdatedIp { set; get; }

		public bool DelFlag { set; get; }

		public virtual ICollection<ShowTime> ShowTime { set; get;}

        public virtual ICollection<TypeFilmDetail> TypeFilmDetail { set; get; }

        //public virtual ICollection<Tickets> Ticket { set; get; }

    }
}

