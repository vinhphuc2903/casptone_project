using System;
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
using CapstoneProject.Commons.Schemas;
using ShowTimeDb = CapstoneProject.Databases.Schemas.System.Ticket.ShowTime;
using TicketData = CapstoneProject.Databases.Schemas.System.Ticket.Tickets;
using TypeFilmData = CapstoneProject.Databases.Schemas.System.Film.TypeFilmDetail;
using FoodDb = CapstoneProject.Databases.Schemas.System.Food.Foods;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;
using CapstoneProject.Commons;
using CapstoneProject.Areas.ShowTime.Models.Schemas;
using CapstoneProject.Commons.Enum;
using CapstoneProject.Databases.Schemas.System.Ticket;
using Microsoft.IdentityModel.Tokens;
using CapstoneProject.Services;
using CapstoneProject.Areas.Film.Models.FilmAdminModels.Schemas;

namespace CapstoneProject.Areas.ShowTime.Models
{
    public interface IShowtimeModels
    {
        /// <summary>
        /// Lấy danh sách phim theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        Task<ListShowTime> GetListShowtime(SearchCondition searchCondition);
        /// <summary>
        /// Thêm film
        /// </summary>
        /// <param name="showTimeInput"></param>
        /// <returns></returns>
        Task<ResponseInfo> AddShowTime(ShowTimeInput showTimeInput);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderData"></param>
        /// <returns></returns>
        Task<ResponseInfo> CreateFood(FoodData foodData);

    }
    public class ShowtimeModels : CapstoneProjectModels, IShowtimeModels
    {
        private readonly ILogger<ShowtimeModels> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        private readonly IMediaService _iIMediaService;

        public ShowtimeModels(
            IConfiguration configuration,
            ILogger<ShowtimeModels> logger,
            IServiceProvider provider,
            IMediaService mediaService
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _iIMediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
        }
        public async Task<ResponseInfo> AddShowTime(ShowTimeInput showTimeInput)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            ResponseInfo responseInfo = new ResponseInfo();
            try
            {
                //Kiểm tra số ngày tạo <= 7
                if ((showTimeInput.DateTo.Date.Subtract(showTimeInput.DateFrom.Date).Days) <= 0)
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.DATE_TO_MUST_BE_LAGER_DATE_FROM;
                    return responseInfo;
                }
                //Kiểm tra số ngày tạo <= 7
                if ((showTimeInput.DateTo.Date.Subtract(showTimeInput.DateFrom.Date).Days) > 7)
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.DATE_CREATE_IS_7_DAY;
                    return responseInfo;
                }
                for (DateTime dateInput = showTimeInput.DateFrom.Date; dateInput.Date <= showTimeInput.DateTo.Date; dateInput = dateInput.AddDays(1))
                {
                    //Suất chiếu từ
                    int timeFromShow = showTimeInput.TimeFrom.Minute + showTimeInput.TimeFrom.Hour * 60 + 420;
                    //Tổng thời gian phim chiếu
                    int totalTimeFilm = await _context.Films
                        .Where(x => !x.DelFlag && x.Id == showTimeInput.IdFilm)
                        .Select(x => x.Time)
                        .FirstOrDefaultAsync();
                    //Suất cuối cùng từ
                    int timeToShow = timeFromShow + (showTimeInput.MinOff + totalTimeFilm) * (showTimeInput.CountShow - 1);
                    //Kiểm tra thời gian bắt đầu chiếu > 8h < 1h ngày hôm sau
                    if (timeFromShow < 8 * 60 || timeToShow > 25 * 60)
                    {
                        responseInfo.Code = CodeResponse.HAVE_ERROR;
                        responseInfo.MsgNo = MSG_NO.TIME_SHOW_ERROR;
                        return responseInfo;    
                    }
                    for (int count = 0; count < showTimeInput.CountShow; count++)
                    {
                        // Tạo đối tượng DateTime với giờ 0:00:00
                        DateTime startOfDay = new DateTime(dateInput.Year, dateInput.Month, dateInput.Day, 0, 0, 0);
                        // Tạo đối tượng DateTime với giờ 23:59:59
                        DateTime endOfDay = new DateTime(dateInput.Year, dateInput.Month, dateInput.Day, 23, 59, 59);
                       
                        //Suất chiếu từ
                        int timeFrom = timeFromShow + count*(totalTimeFilm + showTimeInput.MinOff);
                        //Suất chiếu đến
                        int timeTo = timeFrom + totalTimeFilm + showTimeInput.MinOff;
                        //Kiểm tra có xuất chiếu chưa
                        var showTime = await _context.ShowTime
                            .Where(x => !x.DelFlag
                                && x.CinemaRoomId == showTimeInput.IdRoom
                                && x.DateShow <= endOfDay
                                && x.FilmId == showTimeInput.IdFilm
                                && x.DateShow >= startOfDay
                                && ((x.FromHour * 60 + x.FromMinus <= timeFrom && x.ToHour * 60 + x.ToMinus > timeFrom)
                                || (x.FromHour * 60 + x.FromMinus < timeTo && x.ToHour * 60 + x.ToMinus >= timeTo))).FirstOrDefaultAsync();
                        if (showTime != null)
                        {
                            responseInfo.Code = CodeResponse.HAVE_ERROR;
                            responseInfo.MsgNo = MSG_NO.SHOWTIME_IS_EXIST;
                            responseInfo.Data.Add("Name", showTime.Name);
                            responseInfo.Data.Add("Code", showTime.Code);
                            return responseInfo;
                        }
                        //if (totalTimeFilm > timeTo - timeFrom + 10)
                        //{
                        //    responseInfo.Code = CodeResponse.HAVE_ERROR;
                        //    responseInfo.MsgNo = MSG_NO.TIME_FILM_LONGER_THAN_SHOW;
                        //    return responseInfo;
                        //}
                        //Lấy tổng số ghế theo phòng
                        var listSeat = await _context.Seats
                            .Where(x => !x.DelFlag && x.CinemaRoomId == showTimeInput.IdRoom)
                            .ToListAsync();
                        //Lay ten phong
                        string nameRoom = await _context.CinemaRooms
                            .Where(x => !x.DelFlag && x.Id == showTimeInput.IdRoom)
                            .Select( x => x.Name)
                            .FirstOrDefaultAsync();
                        //Tên suất chiếu
                        string showName = nameRoom + "/" + showTimeInput.IdFilm.ToString() + (timeFrom /60).ToString();
                        //Mã xuất chiếu
                        string showCode = nameRoom + "/" + showTimeInput.IdFilm.ToString() + "/" + timeFrom.ToString() + "/" + timeTo.ToString();
                        ShowTimeDb showtimeData = new ShowTimeDb()
                        {
                            Name = showName,
                            Code = showCode,
                            FilmId = showTimeInput.IdFilm,
                            CinemaRoomId = showTimeInput.IdRoom,
                            Total = listSeat.Count(),
                            TotalSold = 0,
                            TotalRemain = listSeat.Count(),
                            BranchId = showTimeInput.BranchId,
                            DateShow = dateInput,
                            FromHour = timeFrom / 60,
                            ToHour = timeTo / 60,
                            FromMinus = timeFrom % 60,
                            ToMinus = timeTo % 60,
                        };
                        await _context.ShowTime.AddAsync(showtimeData);
                        await _context.SaveChangesAsync();
                        List<TicketData> listTicket = new List<TicketData>();
                        foreach (var seat in listSeat)
                        {
                            int surcharge = 0;
                            if (dateInput.DayOfWeek == DayOfWeek.Friday || dateInput.DayOfWeek == DayOfWeek.Saturday || dateInput.DayOfWeek == DayOfWeek.Sunday)
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
                                Price = 45000 + surcharge,
                                SeatId = seat.Id,
                                ShowtimeId = showtimeData.Id,
                                Name = seat.SeatCode,
                                Type = "10"
                            };
                            listTicket.Add(tickets);
                        }
                        await _context.Ticket.AddRangeAsync(listTicket);
                        await _context.SaveChangesAsync();
                        transaction = await _context.Database.BeginTransactionAsync();
                        await transaction?.CommitAsync();
                    }
                }
                
                return responseInfo;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ListShowTime> GetListShowtime(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            ResponseInfo responseInfo = new ResponseInfo();
            try
            {
                ListShowTime listShowTime = new ListShowTime();
                var showTimeDatas = _context.ShowTime
                    .Include(x => x.CinemaRooms)
                    .Where(x => !x.DelFlag
                        && !x.CinemaRooms.DelFlag
                        && (String.IsNullOrEmpty(searchCondition.Id.ToString())
                        || x.Id == searchCondition.Id)
                        && (String.IsNullOrEmpty(searchCondition.BranchId.ToString())
                        || x.BranchId == searchCondition.BranchId)
                        && (String.IsNullOrEmpty(searchCondition.FilmName)
                        || EF.Functions.Collate(x.Film.Name.Replace(" ", ""), "Latin1_General_CI_AI").Contains(EF.Functions.Collate(searchCondition.FilmName.Replace(" ", ""), "Latin1_General_CI_AI")))
                        && (searchCondition.DateFrom == null
                        || x.DateShow >= searchCondition.DateFrom.Value.Date)
                        && (searchCondition.DateTo == null
                        || x.DateShow.Date <= searchCondition.DateTo.Value.Date)
                        && (String.IsNullOrEmpty(searchCondition.CinemeRoomId.ToString())
                        || x.CinemaRoomId == searchCondition.CinemeRoomId)
                    )
                    .Select(x => new ShowTimeData()
                    {
                        Id = x.Id,
                        DateShow = x.DateShow,
                        BranchId = x.BranchId,
                        BranchName = x.Branches.Name,
                        CinemeRoom = x.CinemaRooms.Name,
                        FilmName = x.Film.Name,
                        ShowtimeName = x.Name,
                        ShowtimeCode = x.Code,
                        FromHour = x.FromHour,
                        ToHour = x.ToHour,
                        FromMinus = x.FromMinus,
                        ToMinus = x.ToMinus,
                        TotalTicketSold = x.TotalSold,
                        TotalTicketRemain = x.TotalRemain,
                        TotalTicket = x.Total,
                    })
                    .OrderByDescending(x => x.DateShow)
                    .ThenByDescending(x => x.BranchId)
                    .ThenByDescending(x => x.CinemeRoom)
                    .ThenByDescending(x => x.FilmName)
                    .ThenBy(x => x.FromHour);
                var totalCount = showTimeDatas.Count();
                listShowTime.Paging = new Paging(totalCount, searchCondition.CurrentPage, searchCondition.PageSize);
                listShowTime.showTimeDatas = await showTimeDatas.Skip((searchCondition.CurrentPage - 1) * searchCondition.PageSize)
                                                .Take(searchCondition.PageSize)
                                                .ToListAsync();
                return listShowTime;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }

        public async Task<ResponseInfo> CreateFood(FoodData foodData)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            ResponseInfo responseInfo = new ResponseInfo();
            try
            {
                //Upload ảnh lên s3
                string linkImage = await _iIMediaService.UploadImageToS3(foodData.ImageLink, true, 254, 381);

                FoodDb foods = new FoodDb()
                {
                    NameOption1 = foodData.NameOption1,
                    NameOption2 = foodData.NameOption2,
                    Price = foodData.Price,
                    SalePrice = foodData.SalePrice,
                    OriginPrice = foodData.OriginPrice,
                    Type = foodData.Type,
                    Status = foodData.Status,
                    ImageLink = linkImage,
                    SizeId = foodData.SizeId,
                };
                await _context.Foods.AddAsync(foods);
                await _context.SaveChangesAsync();
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

