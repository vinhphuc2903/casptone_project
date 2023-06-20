using System;
namespace CapstoneProject.Areas.Film.Models.FilmModels.Schemas
{
    public class ShowTimeDetail
    {
        public ShowTimeDetail()
        {
            ShowtimeData = new ShowTimeDetailData();
            ListTicketData = new List<TicketData>();
        }
        /// <summary>
        /// Chi tiết lịch chiếu
        /// </summary>
        public ShowTimeDetailData ShowtimeData { get; set; }
        /// <summary>
        /// Danh sách chi tiết vé
        /// </summary>
        public List<TicketData> ListTicketData { get; set; }
        /// <summary>
        /// Chi tiết phòng chiếu
        /// </summary>
        public RoomDetail RoomDetail { get; set; }

    }
}

