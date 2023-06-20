using System;
using CapstoneProject.Commons;

namespace CapstoneProject.Areas.Report.Models.Schemas
{
	public class ListRevenueByDate
	{
		public ListRevenueByDate()
		{
		}
		public TotalRevenueByDate TotalRevenueByDates { get; set; }
		public List<RevenueByDate> ListRevenueByDates { get; set; }
		public Paging Paging { get; set; }
    }
}

