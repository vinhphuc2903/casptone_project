using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases.Schemas.System.Users;

namespace CapstoneProject.Databases.Schemas.Setting
{
    /// <summary>
    /// Bảng các phường, xã trong nước
    /// </summary>
    [Table("Communes")]
    public class Communes : ITable
    {
        public Communes()
        {
            User = new HashSet<User>();
            Branches = new HashSet<Branch>();
        }
        /// <summary>
        /// Id định danh (khóa chính)
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string Id { get; set; }
        /// <summary>
        /// Id đến Quận/Huyện của Phường/Xã
        /// </summary>
        [Required]
        [StringLength(3)]
        public string DistrictId { get; set; }
        /// <summary>
        /// Tên Phường/Xã
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// Tên Phường/Xã bằng tiếng anh (nếu có)
        /// </summary>
        [StringLength(50)]
        public string EngName { get; set; }
        /// <summary>
        /// Cấp của Phường/Xã
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

        public virtual Districts Districts { get; set; }

        public virtual ICollection<User> User { get; set; }

        /// <summary>
        /// Chi nhánh
        /// </summary>
        public virtual ICollection<Branch> Branches { get; set; }
    }
}

