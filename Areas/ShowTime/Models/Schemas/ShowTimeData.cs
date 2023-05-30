using System;
namespace CapstoneProject.Areas.ShowTime.Models.Schemas
{
	public class ShowTimeData
	{
		public ShowTimeData()
		{
		}
		public DateTime? DateShow { get; set; }
		public int? BranchId { get; set; }
        /// <summary>
        /// Id chi nhánh
        /// </summary>
        public string? BranchName { get; set; }
        public string? CinemeRoom { get; set; }
		public string? FilmName { get; set; }
		public string? ShowtimeName { get; set; }
		public string? ShowtimeCode { get; set; }
		public int? FromHour { get; set; }
		public int? ToHour { get; set; }
		public int? FromMinus { get; set; }
		public int? ToMinus { get; set; }
		public int? TotalTicketSold { get; set; }
		public int? TotalTicketRemain { get; set; }
		public int? TotalTicket { get; set; }
	}
}

