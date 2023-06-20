using System;
using CapstoneProject.Commons;

namespace CapstoneProject.Areas.Report.Models.Schemas
{
	public class ListRevenueByOrder
	{
        public ListRevenueByOrder()
        {
        }
        public TotalRevenueByDate TotalRevenueByOrders { get; set; }
        public List<RevenueByOrder> ListRevenueByOrders { get; set; }
        public Paging Paging { get; set; }
    }
}

