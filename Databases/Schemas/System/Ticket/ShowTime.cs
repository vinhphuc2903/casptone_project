using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using CapstoneProject.Databases.Schemas.System.Film;
using CapstoneProject.Databases.Schemas.System.CinemaRoom;
using CapstoneProject.Databases.Schemas.Setting;
using CapstoneProject.Databases.Schemas.System.Order;

namespace CapstoneProject.Databases.Schemas.System.Ticket
{
    [Table("ShowTime")]
    public partial class ShowTime : TableHaveIdInt, ITable
    {
        public ShowTime()
        {
            //Tickets = new HashSet<Tickets>();
            Orders = new HashSet<Orders>();
        }
        /// <summary>
        /// Ten xuat chieu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ma xuat chieu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Mã film theo suất chiếu
        /// </summary>
		public int FilmId { get; set; }
        /// <summary>
        /// Mã phòng chiếu
        /// </summary>
        public int CinemaRoomId { get; set; }
        /// <summary>
        /// Tổng vé phát ra
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Tổng vé đã bán
        /// </summary>
        public int TotalSold { get; set; }
        /// <summary>
        /// Tổng vé còn lại
        /// </summary>
        public int TotalRemain { get; set; }
        /// <summary>
        /// Ngày chiếu
        /// </summary>
        public DateTime DateShow { get; set; }
        /// <summary>
        /// Từ giờ
        /// </summary>
        public int FromHour { get; set; }
        /// <summary>
        /// Đến giờ
        /// </summary>
        public int ToHour { get; set; }
        /// <summary>
        /// Từ phút
        /// </summary>
        public int FromMinus { get; set; }
        /// <summary>
        /// Đến phút
        /// </summary>
        public int ToMinus { get; set; }
        /// <summary>
        /// Mã chi nhánh
        /// </summary>
        public int? BranchId { get; set; }

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

        public virtual Films Film { get; set; }

        public virtual CinemaRooms CinemaRooms { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
        //public virtual ICollection<Tickets> Tickets { get; set; }
    }
}

