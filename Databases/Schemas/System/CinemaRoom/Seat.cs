using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.Ticket;

namespace CapstoneProject.Databases.Schemas.System.CinemaRoom
{
    [Table("Seat")]
    public partial class Seat : TableHaveIdInt, ITable
    {
        public Seat()
        {
            Ticket = new HashSet<Tickets>();
        }
        /// <summary>
        /// Mã ghế ngồi
        /// </summary>
        public string SeatCode { get; set; }
        /// <summary>
        /// Mã phòng
        /// </summary>
        public int CinemaRoomId { get; set; }
        /// <summary>
        /// Loại ghế:
        /// - 10: ghế thường
        /// - 20: ghế vip
        /// - 30: ghế đôi
        /// </summary>
        public int Type { get; set; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string? CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual ICollection<Tickets> Ticket { set; get; }

        public virtual CinemaRooms CinemaRoom { set; get; }

    }
}

