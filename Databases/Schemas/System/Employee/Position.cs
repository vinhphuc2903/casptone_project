using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases.Schemas.System.Employee;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.Setting;

namespace CapstoneProject.Databases.Schemas.System.Employee
{
    [Table("Positions")]
    public partial class Position : TableHaveIdInt, ITable
    {
        public Position()
        {
            Employees = new HashSet<Employees>();
        }
        /// <summary>
        /// Mã chức vụ
        /// </summary>
        [StringLength(5)]
        public string Code { set; get; }

        public string Name { set; get; }

        public string Note { set; get; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string UpdatedIp { set; get; }

        public bool DelFlag { set; get; }


        public virtual ICollection<Employees> Employees { set; get; }
    }
}

