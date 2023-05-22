using System;
namespace CapstoneProject.Areas.Film.Models.FilmModels.Schemas
{
	public class TicketData
	{
        public int Id { get; set; }
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
        public string SeatName { get; set; }
        // <summary>
        /// Id Suat chieu
        /// </summary>
        public int ShowtimeId { get; set; }
    }
}

