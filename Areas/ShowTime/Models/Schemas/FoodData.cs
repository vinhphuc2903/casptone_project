using System;
namespace CapstoneProject.Areas.ShowTime.Models.Schemas
{
	public class FoodData
	{
		public FoodData()
		{
		}
        public string NameOption1 { get; set; }

        public string? NameOption2 { get; set; }

        public int Price { get; set; }

        public int? SalePrice { get; set; }

        public int? OriginPrice { get; set; }
        /// <summary>
        /// Type:
        /// - 10: Bắp
        /// - 20: Oshi  -Bimbim
        /// - 30: Nước ngọt
        /// - 40: Combo
        /// </summary>
        public int Type { get; set; }

        public string? Status { get; set; }

        public IFormFile ImageLink { get; set; }

        public int? SizeId { get; set; }
    }
}

