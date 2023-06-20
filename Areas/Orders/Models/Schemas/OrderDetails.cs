using System;
namespace CapstoneProject.Areas.Orders.Models.Schemas
{
	public class OrderDetails
	{
		public OrderDetails()
		{
		}
		public int OrderId { get; set; }
		public string OrderCode { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
        public DateTime? PaymentAt { get; set; }
        public string FilmName { get; set; }
		public int? Total { get; set; }
		public string StatusOrder { get; set; }
		public string? StatusPayment { get; set; }
		public List<string>? ChairTicket { get; set; }
		public List<FoodOrder>? FoodOrders { get; set; }
    }
	public class FoodOrder
	{
		public int Quantity { get; set; }

		public string Name { get; set; }
	}
}

