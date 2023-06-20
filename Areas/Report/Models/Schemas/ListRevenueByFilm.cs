using System;
using CapstoneProject.Commons;

namespace CapstoneProject.Areas.Report.Models.Schemas
{
	public class ListRevenueByFilm
	{
		public ListRevenueByFilm()
		{
		}
        public TotalRevenueByDate TotalRevenueByDates { get; set; }
        public List<RevenueByFilm> RevenueByFilms { get; set; }
        public Paging Paging { get; set; }
    }
}

