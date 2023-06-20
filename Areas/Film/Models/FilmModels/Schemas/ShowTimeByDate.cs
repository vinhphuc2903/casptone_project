using System;
namespace CapstoneProject.Areas.Film.Models.FilmModels.Schemas
{
	public class ShowTimeByDate
	{
		public ShowTimeByDate()
		{
            
		}

        public FilmInfo filmInfo { get; set; }

        public List<ShowTimeDetailByDate> showTimeDetailByDates { get; set; }

    }
	public class ShowTimeDetailByDate
	{
        public int Id { get; set; }

        /// <summary>
        /// Từ giờ
        /// </summary>
        public int FromHour { get; set; }
        /// <summary>
        /// Đến giờ
        /// </summary>
        public int ToHour { get; set; }
        /// <summary>
        /// Từ phút
        /// </summary>
        public int FromMinus { get; set; }
        /// <summary>
        /// Đến phút
        /// </summary>
        public int ToMinus { get; set; }

    }
}

