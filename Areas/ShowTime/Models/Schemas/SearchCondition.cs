using System;
namespace CapstoneProject.Areas.ShowTime.Models.Schemas
{
	public class SearchCondition
    {
        public int? Id { get; set; }
		/// <summary>
		/// Ngày từ
		/// </summary>
		public DateTime? DateFrom { get; set; }
		/// <summary>
		/// Ngày đến
		/// </summary>
		public DateTime? DateTo { get; set; }
		/// <summary>
		/// Tên film
		/// </summary>
		public string? FilmName { get; set; }
		/// <summary>
		/// Id chi nhánh
		/// </summary>
		public int? BranchId { get; set; }
        /// <summary>
        /// Id mã phong chieu
        /// </summary>
        public int? CinemeRoomId { get; set; }
        /// <summary>
        /// Trang hiện tại.
        /// </summary>
        /// <value>1</value>
        public int CurrentPage { set; get; }

        /// <summary>
        /// Số dòng trên một trang.
        /// </summary>
        /// <value>20</value>
        public int PageSize { set; get; }

        public SearchCondition()
        {
            CurrentPage = 1;
            PageSize = 20;
        }
    }
}

