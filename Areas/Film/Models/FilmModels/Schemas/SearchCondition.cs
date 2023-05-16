using System;
namespace CapstoneProject.Areas.Film.Models.FilmModels.Schemas
{
	public class SearchCondition
	{
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

        public SearchCondition()
        {
            CurrentPage = 1;
            PageSize = 20;
        }
    }
}

