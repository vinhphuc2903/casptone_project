using System;
using System.Drawing.Printing;
using CapstoneProject.Commons;

namespace CapstoneProject.Areas.Orders.Models.Schemas
{
	public class SearchCondition
	{
        public SearchCondition()
        {
            CurrentPage = 1;
            PageSize = 20;
        }
        public int? OrderId { get; set; }
        /// <summary>
        /// Trang hiện tại.
        /// </summary>
        /// <value>1</value>
        public int CurrentPage { set; get; }

        /// <summary>
        /// Số dòng trên một trang.
        /// </summary>
        /// <value>20</value>
        public int PageSize { set; get; }
    }
}

