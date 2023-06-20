using System;
namespace CapstoneProject.Areas.Report.Models.Schemas
{
	public class RevenueByFilm
	{
		public RevenueByFilm()
		{
		}
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Director { get; set; }
        public string? Actor { get; set; }
        public string? DateStart { get; set; }
        public string? DateEnd { get; set; }
        public string? Status { get; set; }
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

