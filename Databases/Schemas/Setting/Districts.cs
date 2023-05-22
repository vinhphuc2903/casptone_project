using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnvDTE;
using CapstoneProject.Databases.Schemas.System.Users;

namespace CapstoneProject.Databases.Schemas.Setting
{
	/// <summary>
	/// Bảng các quận, huyện trong nước
	/// </summary>
	[Table("Districts")]
	public class Districts : ITable
    {
        public Districts()
        {
            Communes = new HashSet<Communes>();
            User = new HashSet<User>();
            Branches = new HashSet<Branch>();
        }
        /// <summary>
        /// Id định danh (khóa chính)
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string Id { get; set; }
        /// <summary>
        /// Id đến Tỉnh/Thành phố của Quận/Huyện
        /// </summary>
        [Required]
        [StringLength(2)]
        public string ProvinceId { get; set; }
        /// <summary>
        /// Tên của Quận/Huyện
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// Tên Quận/Huyện bằng tiếng anh (nếu có)
        /// </summary>
        [StringLength(50)]
        public string EngName { get; set; }
        /// <summary>
        /// Cấp của Quận/Huyện
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
        /// Các Xã trong Quận/Huyện
        /// </summary>
        public virtual ICollection<Communes> Communes { get; set; }

        // <summary>
        /// Tỉnh/Thành phố của Quận/Huyện
        /// </summary>
        public virtual Provinces Provinces { get; set; }

        public virtual ICollection<User> User { get; set; }
        /// <summary>
        /// Chi nhánh
        /// </summary>
        public virtual ICollection<Branch> Branches { get; set; }
    }
}

