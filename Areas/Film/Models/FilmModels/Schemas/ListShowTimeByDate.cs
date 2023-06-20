using System;
using CapstoneProject.Commons;

namespace CapstoneProject.Areas.Film.Models.FilmModels.Schemas
{
	public class ListShowTimeByDate
	{
		public ListShowTimeByDate()
		{
		}
        public DateTime DateRecord { get; set; }
		public List<ShowTimeByDate> showTimeByDates { get; set; }
		public Paging Paging { get; set; }
    }
}

