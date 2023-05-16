﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CapstoneProject.Databases;
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
        public string? Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

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
    }
}

