using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.Setting;
using CapstoneProject.Databases.Schemas.System.Users;

namespace CapstoneProject.Databases.Schemas.System.Employee
{
    [Table("Employees")]
    public partial class Employees : TableHaveIdInt, ITable
    {
        /// <summary>
        /// Id user nhân viên
        /// </summary>
        public int? UserId { set; get; }
        [StringLength(50)]
        public string? EmployeeCode { get; set; }
        
        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public int? BranchId { get; set; }

        public int? PositionId { set; get; }

        /// <summary>
        /// Ngay vao lam
        /// </summary>
        public DateTime DateStart { set; get; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual Branch? Branches { set; get; }

        public virtual User User { set; get; }

        public virtual Position? Positions { get; set; }
    }
}

