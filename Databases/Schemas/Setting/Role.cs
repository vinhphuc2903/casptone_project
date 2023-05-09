using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases.Schemas.System.Users;

namespace CapstoneProject.Databases.Schemas.Setting
{
    [Table("Roles")]

    public partial class Role : ITable
    {
        public Role()
		{
            Users = new HashSet<UserRole>();
        }
        [Key]
        [StringLength(5)]
        public string Id { set; get; }

        [Required]
        [StringLength(50)]
        public string Name { set; get; }

        public int Type { set; get; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual ICollection<UserRole> Users { set; get; }
    }
}

