using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using CapstoneProject.Databases.Schemas.System.Food;

namespace CapstoneProject.Databases.Schemas.Setting
{
    [Table("Size")]
    public partial class Size : TableHaveIdInt, ITable
    {
        public Size()
        {
            Foods = new HashSet<Foods>();
        }

		public string Name { get; set; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string? CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual ICollection<Foods> Foods { get; set; }
    }
}

