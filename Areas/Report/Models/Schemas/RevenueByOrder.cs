using System;
namespace CapstoneProject.Areas.Report.Models.Schemas
{
	public class RevenueByOrder
	{
		public RevenueByOrder()
		{
		}
        public string? Name { get; set; }
        public string? OrderCode { get; set; }
        public string? PaymentAt { get; set; }
        public double? TotalTicketSold { get; set; }
        public double? TotalFoodSold { get; set; }
        public double? TotalRevenueTicket { get; set; }
        public double? TotalRevenueFood { get; set; }
        public double? RevenueBeforeDiscount
        {
            get { return TotalRevenueFood + TotalRevenueTicket; }
        }
        public double? DiscountPrice { get; set; }
        public double? RevenueAfterDiscount
        {
            get { return TotalRevenueFood + TotalRevenueTicket - DiscountPrice; }
        }
    }
}

