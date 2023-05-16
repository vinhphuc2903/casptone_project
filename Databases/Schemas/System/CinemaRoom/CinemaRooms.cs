using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.Ticket;

namespace CapstoneProject.Databases.Schemas.System.CinemaRoom
{
    [Table("CinemaRooms")]
    public partial class CinemaRooms : TableHaveIdInt, ITable
    {
        public CinemaRooms()
        {
            Seats = new HashSet<Seat>();
            Showtimes = new HashSet<ShowTime>();
        }
        /// <summary>
        /// Tổng số cột
        /// </summary>
        public int TotalColumn { get; set; }
        /// <summary>
        /// Tổng số hàng
        /// </summary>
        public int TotalRow { get; set; }
        /// <summary>
        /// Tổng số ghế
        /// </summary>
        public int TotalSeat { get; set; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string? CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual ICollection<Seat> Seats { set; get; }

        public virtual ICollection<ShowTime> Showtimes { set; get; }
    }
}

