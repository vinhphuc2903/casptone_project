using System;
namespace CapstoneProject.Commons
{
    /// <summary>
    /// Quản lý việc phân trang, lưu các tham số dùng cho việc phân trang của 1 danh sách.
    /// <para>Author: VinhPhuc</para>
    /// <para>Created: 15/05/2023</para>
    /// </summary>
    public class Paging
    {
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int CurrentPage { set; get; }

        /// <summary>
        /// Tổng số trang hiển thị
        /// </summary>
        public int TotalPages { set; get; }

        /// <summary>
        /// Số dòng tối đa trên 1 trang
        /// </summary>
        public int NumberOfRecord { set; get; }

        /// <summary>
        /// Tổng số các dòng
        /// </summary>
        public int TotalRecord { set; get; }

        /// <summary>
        /// Hàm khởi tạo mặc định để gán các tham số mặc định khi khởi tạo 1 biến để lưu phân trang.
        /// <para>Author: VinhPhuc</para>
        /// <para>Created: 15/02/2023</para>
        /// </summary>
        public Paging()
        {
            this.NumberOfRecord = 10;
            this.TotalPages = 1;
            this.CurrentPage = 1;
            this.TotalRecord = 0;
        }
        /// <summary>
        /// Hàm khởi tạo có các tham số để gán các tham số theo ý người khởi tạo.
        /// <para>Author: VinhPhuc</para>
        /// <para>Created: 15/05/2023</para>
        /// </summary>
        /// <param name="TotalRecord">Tổng số hàng trong 1 bảng</param>
        /// <param name="CurrenPage">Trang hiện tại đang hiển thị</param>
        /// <param name="NumberOfRecord">Số hàng sẽ hiển thị trong 1 trang, mặc định là 30</param>
        public Paging(int TotalRecord, int CurrenPage, int NumberOfRecord = 30)
        {
            this.TotalRecord = TotalRecord;
            this.NumberOfRecord = NumberOfRecord;
            if (this.NumberOfRecord == 0)
            {
                this.NumberOfRecord = TotalRecord;
            }
            this.TotalPages = TotalRecord / this.NumberOfRecord + (TotalRecord % this.NumberOfRecord > 0 ? 1 : 0);
            if (CurrenPage > this.TotalPages)
            {
                CurrenPage = this.TotalPages == 0 ? 1 : this.TotalPages;
            }
            if (CurrenPage < 1)
            {
                CurrenPage = 1;
            }
            this.CurrentPage = CurrenPage;
        }
    }
}
