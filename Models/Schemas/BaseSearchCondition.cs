using System;

namespace FMStyle.RPT.Models.Schemas
{
    public class BaseSearchCondition
    {
        public BaseSearchCondition()
        {
            CurrentPage = 1;
            PageSize = 50;
        }

        /// <summary>
        /// Từ ngày
        /// </summary>
        /// <value></value>
        public DateTime? From { get; set; }

        /// <summary>
        /// Đến ngày
        /// </summary>
        /// <value></value>
        public DateTime? To { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        /// <value></value>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Số record trên 1 page
        /// </summary>
        /// <value></value>
        public int PageSize { get; set; }
    }
}