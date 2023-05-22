using System;
using CapstoneProject.Areas.Film.Models.FilmModels.Schemas;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using CapstoneProject.Areas.Employee.Models.Schemas;
using CapstoneProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using CapstoneProject.Models.Schemas;
using EmployeeData = CapstoneProject.Databases.Schemas.System.Employee.Employees;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using CapstoneProject.Areas.Employee.Models;
using CapstoneProject.Databases.Schemas.System.Film;
using CapstoneProject.Commons.Schemas;
using ShowTimeData = CapstoneProject.Databases.Schemas.System.Ticket.ShowTime;
using TicketData = CapstoneProject.Databases.Schemas.System.Ticket.Tickets;
using TypeFilmData = CapstoneProject.Databases.Schemas.System.Film.TypeFilmDetail;
using CapstoneProject.Areas.Film.Models.FilmAdminModels.Schemas;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;
using CapstoneProject.Commons;
using CapstoneProject.Areas.ShowTime.Models.Schemas;
using CapstoneProject.Commons.Enum;
using CapstoneProject.Databases.Schemas.System.Ticket;

namespace CapstoneProject.Areas.ShowTime.Models
{
	public interface IShowtimeModels
	{
        /// <summary>
        /// Thêm film
        /// </summary>
        /// <param name="showTimeInput"></param>
        /// <returns></returns>
		Task<ResponseInfo> AddShowTime(ShowTimeInput showTimeInput);
	}
	public class ShowtimeModels : CapstoneProjectModels, IShowtimeModels
    {
        private readonly ILogger<ShowtimeModels> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        public ShowtimeModels(
            IConfiguration configuration,
            ILogger<ShowtimeModels> logger,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<ResponseInfo> AddShowTime(ShowTimeInput showTimeInput)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            ResponseInfo responseInfo = new ResponseInfo();
            try
            {
                // Tạo đối tượng DateTime với giờ 0:00:00
                DateTime startOfDay = new DateTime(showTimeInput.DateShow.Year, showTimeInput.DateShow.Month, showTimeInput.DateShow.Day, 0, 0, 0);
                // Tạo đối tượng DateTime với giờ 23:59:59
                DateTime endOfDay = new DateTime(showTimeInput.DateShow.Year, showTimeInput.DateShow.Month, showTimeInput.DateShow.Day, 23, 59, 59);
                //Suất chiếu từ
                int timeFrom = showTimeInput.TimeFrom.Minute + showTimeInput.TimeFrom.Hour * 60;
                //Suất chiếu đến
                int timeTo = showTimeInput.TimeTo.Minute + showTimeInput.TimeTo.Hour * 60;
                //Kiểm tra có xuất chiếu chưa
                var showTime = await _context.ShowTime
                    .Where(x => !x.DelFlag
                        && x.CinemaRoomId == showTimeInput.IdRoom
                        && x.DateShow <= endOfDay
                        && x.DateShow >= startOfDay
                        && ((x.ToHour * 60 + x.ToMinus >= timeFrom && x.ToHour * 60 + x.ToMinus <= timeTo)
                        || (x.FromHour * 60 + x.FromMinus >= timeFrom && x.FromHour * 60 + x.FromMinus <= timeFrom))).FirstOrDefaultAsync();
                if(showTime != null)
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.SHOWTIME_IS_EXIST;
                    responseInfo.Data.Add("Name", showTime.Name);
                    responseInfo.Data.Add("Code", showTime.Code);
                    return responseInfo;
                }
                // Kiểm tra film có dài hơn suất chiếu k
                int totalTimeFilm = await _context.Films
                    .Where(x => !x.DelFlag && x.Id == showTimeInput.IdFilm)
                    .Select(x => x.Time)
                    .FirstOrDefaultAsync();
                if(totalTimeFilm > timeTo - timeFrom + 10)
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.TIME_FILM_LONGER_THAN_SHOW;
                    return responseInfo;
                }    
                //Lấy tổng số ghế theo phòng
                var listSeat = await _context.Seats
                    .Where(x => !x.DelFlag && x.CinemaRoomId == showTimeInput.IdRoom)
                    .ToListAsync();
                
                ShowTimeData showtimeData = new ShowTimeData()
                {
                    Name = showTimeInput.Name,
                    Code = showTimeInput.Code,
                    FilmId = showTimeInput.IdFilm,
                    CinemaRoomId = showTimeInput.IdRoom,
                    Total = listSeat.Count(),
                    TotalSold = 0,
                    TotalRemain = listSeat.Count(),
                    DateShow = showTimeInput.DateShow,
                    FromHour = showTimeInput.TimeFrom.Hour,
                    ToHour = showTimeInput.TimeTo.Hour,
                    FromMinus = showTimeInput.TimeFrom.Minute,
                    ToMinus = showTimeInput.TimeTo.Minute,
                };
                await _context.ShowTime.AddAsync(showtimeData);
                await _context.SaveChangesAsync();
                List<TicketData> listTicket = new List<TicketData>();
                foreach(var seat in listSeat)
                {
                    int surcharge = 0;
                    if (showTimeInput.DateShow.DayOfWeek == DayOfWeek.Friday || showTimeInput.DateShow.DayOfWeek == DayOfWeek.Saturday || showTimeInput.DateShow.DayOfWeek == DayOfWeek.Sunday)
                    {
                        surcharge = 15000;
                    }
                    switch (seat.Type)
                    {
                        case 20:
                            surcharge += surcharge + 20000;
                            break;
                        case 30:
                            surcharge += surcharge * 2 + 45000;
                            break;
                    }

                    TicketData tickets = new TicketData()
                    {
                        Price = 45000 * surcharge,
                        SeatId = seat.Id,
                        ShowtimeId = showtimeData.Id,
                        Name = seat.SeatCode
                    };
                    listTicket.Add(tickets);
                }
                await _context.Ticket.AddRangeAsync(listTicket);
                await _context.SaveChangesAsync();
                transaction = await _context.Database.BeginTransactionAsync();
                await transaction?.CommitAsync();
                return responseInfo;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }

    }
}

