using System;
namespace CapstoneProject.Areas.Film.Models.FilmModels.Schemas
{
	public class SearchCondition
	{
        public string? Name { get; set; }
        /// <summary>
        /// Thể loại film
        /// </summary>
        public int? TypeFilm { get; set; }
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

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string? Status { get; set; }

        public SearchCondition()
        {
            CurrentPage = 1;
            PageSize = 20;
        }
    }
}

