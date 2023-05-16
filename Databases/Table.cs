using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapstoneProject.Databases
{
    /// <summary>
    /// Dữ liệu chung cho các table trong hệ thống (thông tin việc tạo, update và delete dữ liệu).
    /// <para>Author: VinhPhuc</para>
    /// <para>Created: 03/05/2023</para>
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Ngày tạo dữ liệu
        /// </summary>
        public DateTimeOffset CreatedAt { set; get; }
        /// <summary>
        /// Người tạo dữ liệu
        /// </summary>
        public int CreatedBy { set; get; }
        /// <summary>
        /// Ip của máy tạo dữ liệu
        /// </summary>
        [StringLength(50)]
        public string CreatedIp { set; get; }
        /// <summary>
        /// Ngày cập nhật dữ liệu
        /// </summary>
        public DateTimeOffset? UpdatedAt { set; get; }
        /// <summary>
        /// Người cập nhật dữ liệu
        /// </summary>
        public int? UpdatedBy { set; get; }
        /// <summary>
        /// Ip của máy đã cập nhật dữ liệu
        /// </summary>
        [StringLength(50)]
        public string? UpdatedIp { set; get; }
        /// <summary>
        /// Cờ xóa dữ liệu
        /// </summary>
        public bool DelFlag { set; get; }
    }

    /// <summary>
    /// Bảng có khóa chính là kiểu int và tự động tăng.
    /// <para>Author: VinhPhuc</para>
    /// <para>Created: 03/05/2023</para>
    /// </summary>
    public class TableHaveIdInt
    {
        /// <summary>
        /// Id định danh (khóa chính)
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public int Id { set; get; }
    }
}
