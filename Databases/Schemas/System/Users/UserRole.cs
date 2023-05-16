using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using CapstoneProject.Databases.Schemas.System.Employee;
using CapstoneProject.Databases.Schemas.Setting;

namespace CapstoneProject.Databases.Schemas.System.Users
{
    [Table("UserRole")]
    public class UserRole : ITable
    {
        [Key]
        [Column(Order = 1)]
        public int UserId { set; get; }

        [Key]
        [Column(Order = 2)]
        [StringLength(5)]
        public string RoleId { set; get; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual User User { set; get; }

        public virtual Role Role { set; get; }
    }
}

