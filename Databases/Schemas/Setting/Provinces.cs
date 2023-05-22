using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Numerics;
using EnvDTE;
using CapstoneProject.Databases.Schemas.System.Users;

namespace CapstoneProject.Databases.Schemas.Setting
{
    /// <summary>
    /// Bảng lưu các tỉnh trong nước
    /// </summary>
    [Table("Provinces")]
    public class Provinces : ITable
    {
        public Provinces()
        {
            User = new HashSet<User>();
            Districts = new HashSet<Districts>();
            Branches = new HashSet<Branch>();
        }
        /// <summary>
        /// Id định danh (khóa chính)
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string Id { get; set; }
        /// <summary>
        /// Id Khu vực khu vực Tỉnh/Thành phố(khóa phụ)
        /// </summary>
        [StringLength(1)]
        public string RegionId { get; set; }
        /// <summary>
        /// Tên Tỉnh/Thành phố
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// Tên Tỉnh/Thành phố bằng tiếng anh (nếu có)
        /// </summary>
        [StringLength(50)]
        public string EngName { get; set; }
        /// <summary>
        /// Cấp của Tỉnh/Thành phố
        /// </summary>
        [StringLength(50)]
        public string Level { get; set; }

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
        public string UpdatedIp { set; get; }
        /// <summary>
        /// Cờ xóa dữ liệu
        /// </summary>
        public bool DelFlag { set; get; }
        /// <summary>
        /// Các Quận/Huyện trong Tỉnh/Thành phố
        /// </summary>
        public virtual ICollection<Districts> Districts { get; set; }

        public virtual ICollection<User> User { get; set; }
        /// <summary>
        /// Chi nhánh
        /// </summary>
        public virtual ICollection<Branch> Branches { get; set; }
    }
}

