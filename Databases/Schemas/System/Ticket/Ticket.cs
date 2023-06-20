using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.CinemaRoom;
using CapstoneProject.Databases.Schemas.System.Film;
using CapstoneProject.Databases.Schemas.System.Order;

namespace CapstoneProject.Databases.Schemas.System.Ticket
{
    [Table("Ticket")]
    public partial class Tickets : TableHaveIdInt, ITable
    {
        public Tickets()
        {
            OrderTicketDetail = new HashSet<OrderTicketDetail>();
        }
        /// <summary>
        /// Ten
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gia ve
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// Id cho ngoi
        /// </summary>
        public int SeatId { get; set; }
        /// <summary>
        /// Id Suat chieu
        /// </summary>
        public int ShowtimeId { get; set; }
        /// <summary>
        /// Loại vé
        /// 10: Chưa bán
        /// 20: Đã bán
        /// 30: Đang có người chọn
        /// </summary>
        public string? Type { get; set; }

        public DateTime? OrderAt { get; set; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        //public virtual Films Film { set; get; }

        public virtual Seat Seat { set; get; }

        //public virtual ShowTime ShowTime { set; get; }

        public virtual ICollection<OrderTicketDetail> OrderTicketDetail { set; get; }

    }
}

