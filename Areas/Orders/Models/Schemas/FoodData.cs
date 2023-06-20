using System;
namespace CapstoneProject.Areas.Orders.Models.Schemas
{
	public class FoodData
	{
		public FoodData()
		{
		}
        public int Id { get; set; }
        public string NameOption1 { get; set; }

        public string? NameOption2 { get; set; }

        public int Price { get; set; }

        public int? SalePrice { get; set; }

        public int? OriginPrice { get; set; }

        public int Type { get; set; }

        public string? Status { get; set; }

        public string ImageLink { get; set; }

        public int? SizeId { get; set; }
    }
}

