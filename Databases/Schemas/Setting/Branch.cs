using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnvDTE;
using CapstoneProject.Databases.Schemas.System.CinemaRoom;
using CapstoneProject.Databases.Schemas.System.Order;
using CapstoneProject.Databases.Schemas.System.Employee;
using CapstoneProject.Databases.Schemas.System.Ticket;

namespace CapstoneProject.Databases.Schemas.Setting
{
    /// <summary>
    /// Bảng các chi nhánh
    /// </summary>
    [Table("Branches")]
    public class Branch : TableHaveIdInt, ITable
    {
        public Branch()
        {
            CinemaRoom = new HashSet<CinemaRooms>();
            Orders = new HashSet<Orders>();
            Employee = new HashSet<Employees>();
            ShowTimes = new HashSet<ShowTime>();
        }
        [Required]
        [StringLength(100)]
        public string Name { set; get; }

        [Required]
        [StringLength(50)]
        public string Code { set; get; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(5)]
        public string CommuneId { get; set; }

        [StringLength(3)]
        public string DistrictId { get; set; }

        [StringLength(2)]
        public string ProvinceId { get; set; }

        [StringLength(50)]
        public string Phone { set; get; }

        [StringLength(255)]
        public string Email { set; get; }

        public string? BrackgroundImageLink { get; set; }
        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual Communes Commune { set; get; }

        public virtual Districts District { set; get; }

        public virtual Provinces Province { set; get; }

        public virtual IEnumerable<Employees> Employee { get; set; }

        public virtual IEnumerable<Orders> Orders { get; set; }

        public virtual IEnumerable<CinemaRooms> CinemaRoom { get; set; }

        public virtual IEnumerable<ShowTime> ShowTimes { get; set; }

    }
}

