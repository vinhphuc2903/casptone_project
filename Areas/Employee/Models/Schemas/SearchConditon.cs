using System;
namespace CapstoneProject.Areas.Employee.Models.Schemas
{
	public class SearchConditon
	{
		public SearchConditon()
		{

		}
		public int? Id { get; set; }

		public string? Name { get; set; }

		public DateTime? DateStart { get; set; }

		public int? BranchId { get; set; }

		public int? PositionId { get; set; }

		public string? EmployeeCode { get; set; }
    }
}

