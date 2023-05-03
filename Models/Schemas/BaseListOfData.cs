using System.Collections.Generic;

namespace FMStyle.RPT.Models.Schemas
{
    public abstract class BaseListOfData<T>
    {
        public BaseListOfData()
        {
            Data = new List<T>();
            //Paging = new Paging();
        }

        /// <summary>
        /// Danh sách data
        /// </summary>
        /// <value></value>
        public List<T> Data { get; set; }

        /// <summary>
        /// Thông tin phân trang: page, page size
        /// </summary>
        /// <value></value>
        //public Paging Paging { get; set; }
    }
}