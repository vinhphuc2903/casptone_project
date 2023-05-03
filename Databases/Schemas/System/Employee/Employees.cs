using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CapstoneProject.Databases;

namespace CapstoneProject.Databases.Schemas.System.Employee
{
    [Table("Employees")]
    public class Employees : TableHaveIdInt, ITable
    {

        [StringLength(50)]
        public string? EmployeeCode { get; set; }
        [StringLength(50)]
        public string? Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string UpdatedIp { set; get; }

        public bool DelFlag { set; get; }
    }
}

