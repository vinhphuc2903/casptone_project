using System;
namespace CapstoneProject.Areas.Orders.Models.Schemas
{
	public class OrderData
	{
		public OrderData()
		{
		}
        public List<int> ListTicketId { get; set; }
        public List<FoodDetail> ListFoodDetail { get; set; }
        public int ShowTimeId { get; set; }
    }
    public class TicketDetail
    {
        public int TicketId { get; set; }
    }

    public class FoodDetail
    {
        public int FoodId { get; set; }

        public int Quantity { get; set; }
    }
}

