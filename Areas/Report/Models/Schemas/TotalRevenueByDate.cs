using System;
namespace CapstoneProject.Areas.Report.Models.Schemas
{
	public class TotalRevenueByDate
	{
		public TotalRevenueByDate()
		{
		}
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

