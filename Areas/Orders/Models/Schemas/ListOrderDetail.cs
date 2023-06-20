using System;
using CapstoneProject.Commons;

namespace CapstoneProject.Areas.Orders.Models.Schemas
{
	public class ListOrderDetail
	{
		public ListOrderDetail()
		{
            Paging = new Paging();
        }
        public List<OrderDetails> ListOrderDetails { get; set; }
        public Paging Paging { get; set; }
    }
}

