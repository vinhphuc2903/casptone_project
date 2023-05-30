using System;
using CapstoneProject.Commons;

namespace CapstoneProject.Areas.ShowTime.Models.Schemas
{
	public class ListShowTime
	{
		public ListShowTime()
		{
		}
		public List<ShowTimeData> showTimeDatas { get; set; }
        public Paging Paging { get; set; }

    }
}

