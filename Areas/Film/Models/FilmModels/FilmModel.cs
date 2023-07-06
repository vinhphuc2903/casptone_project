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
using FilmData = CapstoneProject.Databases.Schemas.System.Film.Films;
using TypeFilmData = CapstoneProject.Databases.Schemas.System.Film.TypeFilmDetail;
using TypeFilm = CapstoneProject.Databases.Schemas.System.Film.TypeFilm;
using TicketDb = CapstoneProject.Databases.Schemas.System.Ticket.Tickets;
using CapstoneProject.Areas.Film.Models.FilmAdminModels.Schemas;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;
using CapstoneProject.Commons;
using CapstoneProject.Areas.Users.Models.UserModel.Schemas;
using CapstoneProject.Databases.Schemas.System.Ticket;
using CapstoneProject.Databases.Schemas.System.CinemaRoom;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using Azure;
using CapstoneProject.Commons.CodeMaster;
using CapstoneProject.Commons.Enum;

namespace CapstoneProject.Areas.Film.Models.FilmModels
{
    public interface IFilmModel
    {
        /// <summary>
        /// Lấy danh sách Film
        /// </summary>
        /// <returns></returns>
        Task<List<TypeFilm>> GetListTypeFilm();
        /// <summary>
        /// Lấy danh sách film theo điều kiện tìm kiem
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        Task<ListFilms> GetListFilms(SearchCondition searchCondition);
        /// <summary>
        /// Lấy chi tiết film theo Id
        /// </summary>
        /// <param name="filmId"></param>
        /// <returns></returns>
        Task<FilmDetail> GetDetailFilm(int filmId);
        /// <summary>
        /// Lấy danh sách vé xem phim theo showtime
        /// </summary>
        /// <param name="ShowTimeId"></param>
        /// <returns></returns>
        Task<ShowTimeDetail> GetTicketByShowTime(int ShowTimeId);
        /// <summary>
        /// Lấy danh sách phim và lịch chiếu phim đó theo từng ngày
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        Task<ListShowTimeByDate> GetShowTimeByDateByBranch(SearchCondition searchCondition);
        /// <summary>
        /// Lấy danh sách phim và lịch chiếu phim đó theo từng ngày v2
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        Task<List<ListShowTimeByDate>> GetShowTimeByDateByBranchV2(SearchCondition searchCondition);
    }
    public class FilmModel : CapstoneProjectModels, IFilmModel
    {
        private readonly ILogger<FilmModel> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";

        public FilmModel(
            IConfiguration configuration,
            ILogger<FilmModel> logger,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<List<TypeFilm>> GetListTypeFilm()
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                List<TypeFilm> list = await _context.TypeFilms.Where(x => !x.DelFlag).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ListFilms> GetListFilms(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                ListFilms listFilms = new ListFilms();
                if (searchCondition == null)
                {
                    searchCondition = new SearchCondition();
                }
                DateTime startOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                DateTime endOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                // Phim đang chiếu nhưng không được cập nhật trạng thái

                var listFilm = _context.Films.Where(x => !x.DelFlag
                                                        && (String.IsNullOrEmpty(searchCondition.TypeFilm.ToString())
                                                        || x.TypeFilmDetail.Any(y => y.TypeFilmId == searchCondition.TypeFilm))
                                                        && (String.IsNullOrEmpty(searchCondition.Name)
                                                        || EF.Functions.Collate(x.Name.Replace(" ", ""), "Latin1_General_CI_AI").Contains(EF.Functions.Collate(searchCondition.Name.Replace(" ", ""), "Latin1_General_CI_AI")))
                                                        && (searchCondition.DateStart == null
                                                        || x.DateStart.Date >= searchCondition.DateStart.Value.Date)
                                                        && (searchCondition.DateEnd == null
                                                        || x.DateEnd.Value.Date <= searchCondition.DateEnd.Value.Date)
                                                        //&& (String.IsNullOrEmpty(searchCondition.Status)
                                                        //|| x.Status == searchCondition.Status
                                                        //|| searchCondition.Status == "50")
                                                        //&& (String.IsNullOrEmpty(searchCondition.Status)
                                                        //    || searchCondition.Status != "40"
                                                        //    || (x.DateEnd >= DateTimeOffset.Now)
                                                        //)
                                                        //&& (String.IsNullOrEmpty(searchCondition.Status)
                                                        //    || searchCondition.Status != "50"
                                                        //    || (x.DateEnd < DateTimeOffset.Now && x.Status != "40")
                                                        //)
                                                        // Phim sắp chiếu
                                                        && (
                                                            String.IsNullOrEmpty(searchCondition.Status)
                                                            || searchCondition.Status != "10"
                                                            || (searchCondition.Status == "10"
                                                                && x.DateStart > startOfDay)
                                                        )
                                                        //Phim đang chiếu
                                                        && (String.IsNullOrEmpty(searchCondition.Status)
                                                            || searchCondition.Status != "20"
                                                            || (searchCondition.Status == "20"
                                                                &&
                                                                (
                                                                    (x.DateStart <= startOfDay
                                                                    && x.DateEnd >= startOfDay)
                                                                    || (
                                                                        x.DateStartPostpone <= startOfDay
                                                                        && x.DateExtend >= startOfDay
                                                                    )
                                                                    || (
                                                                        x.DatePostpone == null
                                                                        && x.DateExtend >= startOfDay
                                                                    )
                                                                )
                                                                && (
                                                                    x.DatePostpone == null
                                                                    || x.DatePostpone > startOfDay
                                                                )
                                                            )
                                                        )
                                                        // Phim tạm hoãn
                                                        && (String.IsNullOrEmpty(searchCondition.Status)
                                                            || searchCondition.Status != "30"
                                                            || (searchCondition.Status == "30"
                                                                && x.DatePostpone <= startOfDay
                                                                && (
                                                                    x.DateStartPostpone == null
                                                                    || x.DateStartPostpone <= startOfDay
                                                                )
                                                            )
                                                        )
                                                        // Phim đã chiếu
                                                        && (String.IsNullOrEmpty(searchCondition.Status)
                                                            || searchCondition.Status != "40"
                                                            || (searchCondition.Status == "40"
                                                                && x.DateEnd < startOfDay
                                                                && (
                                                                    x.DateExtend == null
                                                                    || x.DateExtend < startOfDay
                                                                )
                                                            )
                                                        )
                                                    )
                                                    .Select(x => new FilmInfo()
                                                    {
                                                        Id = x.Id,
                                                        Name = x.Name,
                                                        Actor = x.Actor,
                                                        Director = x.Director,
                                                        AgeLimit = x.AgeLimit,
                                                        Time = x.Time,
                                                        Introduce = x.Introduce,
                                                        TrailerLink = x.TrailerLink,
                                                        Country = x.Country,
                                                        BackgroundImage = x.BackgroundImage,
                                                        DateStart = x.DateStart,
                                                        DateEnd = x.DateEnd,
                                                        Status = x.Status,
                                                        Language = x.Language,
                                                        Cost = x.Cost,
                                                        DateRelease = x.DateRelease
                                                    })
                                                    .OrderByDescending(x => x.Id);
                if (listFilm == null)
                {
                    return listFilms;
                }
                var totalCount = listFilm.Count();
                listFilms.Paging = new Paging(totalCount, searchCondition.CurrentPage, searchCondition.PageSize);
                listFilms.ListFilmInfos = await listFilm.Skip((searchCondition.CurrentPage - 1) * searchCondition.PageSize)
                                                .Take(searchCondition.PageSize)
                                                .ToListAsync();

                foreach (var film in listFilms.ListFilmInfos)
                {
                    film.ListTypeFilm = await _context.TypeFilmDetails
                                                    .Where(x => !x.DelFlag && x.FilmId == film.Id)
                                                    .Include(x => x.TypeFilms)
                                                    .Select(x => x.TypeFilms.Name).ToListAsync();
                }
                return listFilms;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<FilmDetail> GetDetailFilm(int filmId)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                DateTime startOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                DateTime endOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).AddDays(3);
                FilmDetail filmDetails = new FilmDetail();
                int timeFrom = DateTime.Now.Minute + DateTime.Now.Hour * 60;
                filmDetails.ListShowtimeInfo = await _context.Branches.Where(x => !x.DelFlag)
                    .Select(x => new ShowtimeInfo
                    {
                        BranchId = x.Id,
                        BranchesName = x.Name,
                        BranchesAddress = x.Address,
                        BranchesProvince = x.Province.Name,
                        BranchesCommune = x.Commune.Name,
                        BranchesDistrict = x.District.Name,
                    })
                    .ToListAsync();
                //Lấy tất cả Xuất chiếu theo từng chi nhanh
                foreach (var showtimeBranch in filmDetails.ListShowtimeInfo)
                {
                    // Lay danh sach ngay co xuat chieu
                    showtimeBranch.ListShowTimeData = await _context.ShowTime
                        .Include(x => x.CinemaRooms)
                        .Where(x => !x.DelFlag
                            && !x.CinemaRooms.DelFlag
                            && x.FilmId == filmId
                            && x.DateShow >= startOfDay
                            && x.DateShow <= endOfDay
                            && x.BranchId == showtimeBranch.BranchId)
                         .Select(x => new ShowTimeData
                         {
                             DateShow = x.DateShow.Date
                         })
                        .GroupBy(x => x.DateShow.Date)
                        //.Select(g => g.OrderBy(x => x.DateShow.Date).Select(x => new ShowTimeData { DateShow = x.DateShow.Date }).First())
                        .OrderBy(g => g.Key)
                        .Select(g => g.First())
                        .ToListAsync();
                    // Lay chi tiet xuat chieu theo tung ngay
                    int index = 0;
                    foreach (var showTime in showtimeBranch.ListShowTimeData)
                    {
                        DateTime startDateShow = new DateTime(showTime.DateShow.Year, showTime.DateShow.Month, showTime.DateShow.Day, 0, 0, 0);
                        int now = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                        showTime.ShowTimeDetailDatas = await _context.ShowTime
                            .Include(x => x.CinemaRooms)
                            .Where(x => !x.DelFlag
                                && x.FilmId == filmId
                                && !x.CinemaRooms.DelFlag
                                && x.DateShow.Date == startDateShow.Date
                                && ((x.FromHour * 60 + x.FromMinus) > now || index != 0)
                            )
                            .OrderBy(g => g.DateShow) // Sắp xếp tăng dần theo ngày
                            .Select(x => new ShowTimeDetailData()
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Code = x.Code,
                                FromHour = x.FromHour,
                                ToHour = x.ToHour,
                                FromMinus = x.FromMinus,
                                ToMinus = x.ToMinus,
                            }).ToListAsync();
                        index++;
                    }
                }
                filmDetails.FilmInfos = await _context.Films
                    .Where(x => !x.DelFlag && x.Id == filmId)
                    .Select(x => new FilmInfo()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Actor = x.Actor,
                        Director = x.Director,
                        AgeLimit = x.AgeLimit,
                        Time = x.Time,
                        Introduce = x.Introduce,
                        TrailerLink = x.TrailerLink,
                        Country = x.Country,
                        BackgroundImage = x.BackgroundImage,
                        DateStart = x.DateStart,
                        DateEnd = x.DateEnd,
                        Status = x.Status,
                        Language = x.Language
                    }).FirstOrDefaultAsync();

                filmDetails.ListTypeFilm = await _context.TypeFilmDetails
                                                    .Where(x => !x.DelFlag && x.FilmId == filmId)
                                                    .Include(x => x.TypeFilms)
                                                    .Select(x => x.TypeFilms.Name).ToListAsync();

                return filmDetails;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ShowTimeDetail> GetTicketByShowTime(int ShowTimeId)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                ShowTimeDetail showTimeDetail = new ShowTimeDetail();
                showTimeDetail.ShowtimeData = await _context.ShowTime
                    .Include(x => x.CinemaRooms)
                    .Where(x => !x.DelFlag
                        && x.Id == ShowTimeId
                        && !x.CinemaRooms.DelFlag
                    )
                    .OrderBy(g => g.DateShow) // Sắp xếp tăng dần theo ngày
                    .Select(x => new ShowTimeDetailData()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        FromHour = x.FromHour,
                        ToHour = x.ToHour,
                        FromMinus = x.FromMinus,
                        ToMinus = x.ToMinus,
                        CinemaRoomId = x.CinemaRoomId,

                    }).FirstOrDefaultAsync();

                //Update ticket
                DateTime currentDateTime = DateTime.Now;
                DateTime dateTimeFiveMinutesAgo = currentDateTime.AddMinutes(-5);

                List<TicketDb> ticketDbs = await _context.Ticket
                   .Where(x => !x.DelFlag
                       && x.ShowtimeId == ShowTimeId
                       && x.Type == "30"
                       && x.OrderAt != null
                       && x.OrderAt <= dateTimeFiveMinutesAgo
                   )
                   .OrderBy(x => x.Id)
                   .ToListAsync();
                foreach (var tickets in ticketDbs)
                {
                    tickets.Type = "10";
                    _context.SaveChangesAsync();
                }
                if (showTimeDetail.ShowtimeData == null) return showTimeDetail;
                showTimeDetail.ListTicketData = await _context.Ticket
                    .Where(x => !x.DelFlag && x.ShowtimeId == ShowTimeId)
                    .OrderBy(x => x.Id)
                    .Select(x => new TicketData()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
                        Price = x.Price,
                        SeatId = x.SeatId,
                        ShowtimeId = x.ShowtimeId,
                        SeatName = x.Seat.SeatCode,
                        TypeSeat = x.Seat.Type
                    })
                    .ToListAsync();
                showTimeDetail.RoomDetail = await _context.CinemaRooms
                    .Include(x => x.Branches)
                    .Include(x => x.Branches.Province)
                    .Include(x => x.Branches.Commune)
                    .Include(x => x.Branches.District)
                    .Where(x => !x.DelFlag && x.Id == showTimeDetail.ShowtimeData.CinemaRoomId)
                    .Select(x => new RoomDetail()
                    {
                        Name = x.Name,
                        TotalColumn = x.TotalColumn,
                        TotalRow = x.TotalRow,
                        TotalSeat = x.TotalSeat,
                        BranchesName = x.Branches.Name,
                        BranchesAddress = x.Branches.Address,
                        BranchesProvince = x.Branches.Province.Name,
                        BranchesCommune = x.Branches.Commune.Name,
                        BranchesDistrict = x.Branches.District.Name,
                    })
                    .FirstOrDefaultAsync();
                return showTimeDetail;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ListShowTimeByDate> GetShowTimeByDateByBranch(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                if (searchCondition == null)
                {
                    searchCondition = new SearchCondition();
                }
                DateTime startOfDay = new DateTime(searchCondition.DateRecord.Value.Year, searchCondition.DateRecord.Value.Month, searchCondition.DateRecord.Value.Day, 0, 0, 0);
                if (searchCondition.DateEnd == null)
                {
                    searchCondition.DateEnd = startOfDay;
                }
                DateTime endOfDay = new DateTime(searchCondition.DateEnd.Value.Year, searchCondition.DateEnd.Value.Month, searchCondition.DateEnd.Value.Day, 23, 59, 59);
                ListShowTimeByDate listShowTimeByDate = new ListShowTimeByDate();
                int timeFrom = DateTime.Now.Minute + DateTime.Now.Hour * 60;
                int now = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                var showTimeByDates = _context.Films
                    .Include(x => x.ShowTime)
                    .ThenInclude(x => x.CinemaRooms)
                    .Where(x => !x.DelFlag && x.DateEnd >= DateTimeOffset.Now
                        //Phim đang chiếu
                        &&
                        (
                            (x.DateStart <= startOfDay
                            && x.DateEnd >= startOfDay)
                            || (
                                x.DateStartPostpone <= startOfDay
                                && x.DateExtend >= startOfDay
                            )
                            || (
                                x.DatePostpone == null
                                && x.DateExtend >= startOfDay
                            )
                        )
                        && (
                            x.DatePostpone == null
                            || x.DatePostpone > startOfDay
                        )
                    )
                    .Select(
                        x => new ShowTimeByDate()
                        {
                            filmInfo = new FilmInfo()
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Actor = x.Actor,
                                Director = x.Director,
                                AgeLimit = x.AgeLimit,
                                Time = x.Time,
                                Introduce = x.Introduce,
                                TrailerLink = x.TrailerLink,
                                Country = x.Country,
                                BackgroundImage = x.BackgroundImage,
                                DateStart = x.DateStart,
                                DateEnd = x.DateEnd,
                                Status = x.Status,
                                Language = x.Language
                            },
                            showTimeDetailByDates = x.ShowTime
                                .Where(x => !x.DelFlag
                                    && x.DateShow >= startOfDay
                                    && x.DateShow <= endOfDay
                                    && x.BranchId == searchCondition.BranchId
                                    && (
                                        searchCondition.CinemeRoomId == null
                                        || x.CinemaRoomId == searchCondition.CinemeRoomId
                                    )
                                    && !x.CinemaRooms.DelFlag
                                    && ((x.FromHour * 60 + x.FromMinus) > now || DateTime.Now.Date != searchCondition.DateRecord.Value.Date))
                                .Select(x => new ShowTimeDetailByDate()
                                {
                                    FromHour = x.FromHour,
                                    ToHour = x.ToHour,
                                    FromMinus = x.FromMinus,
                                    ToMinus = x.ToMinus,
                                    Id = x.Id
                                }).ToList()
                        }
                    );
                if (searchCondition.CinemeRoomId != null)
                {
                    showTimeByDates = showTimeByDates.Where(x => x.showTimeDetailByDates.Any());
                }
                var totalCount = showTimeByDates.Count();
                if (totalCount == 0) return listShowTimeByDate;
                listShowTimeByDate.Paging = new Paging(totalCount, searchCondition.CurrentPage, searchCondition.PageSize);
                listShowTimeByDate.showTimeByDates = await showTimeByDates.Skip((searchCondition.CurrentPage - 1) * searchCondition.PageSize)
                                                .Take(searchCondition.PageSize)
                                                .ToListAsync();
                return listShowTimeByDate;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<List<ListShowTimeByDate>> GetShowTimeByDateByBranchV2(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                if (searchCondition == null)
                {
                    searchCondition = new SearchCondition();
                }
                DateTime startOfDay = new DateTime(searchCondition.DateRecord.Value.Year, searchCondition.DateRecord.Value.Month, searchCondition.DateRecord.Value.Day, 0, 0, 0);
                if (searchCondition.DateEnd == null)
                {
                    searchCondition.DateEnd = startOfDay;
                }
                DateTime endOfDay = new DateTime(searchCondition.DateEnd.Value.Year, searchCondition.DateEnd.Value.Month, searchCondition.DateEnd.Value.Day, 23, 59, 59);
                List<ListShowTimeByDate> listShowTimeByDateReturn = new List<ListShowTimeByDate>();
                int i = 0;
                for (var from = startOfDay; from <= endOfDay; from = from.AddDays(1))
                {
                    i++;
                    if (i == 7) break;
                    DateTime endOfDaySearch = new DateTime(from.Year, from.Month, from.Day, 23, 59, 59);
                    ListShowTimeByDate listShowTimeByDate = new ListShowTimeByDate();
                    listShowTimeByDate.DateRecord = from;
                    int timeFrom = DateTime.Now.Minute + DateTime.Now.Hour * 60;
                    int now = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                    var showTimeByDates = _context.Films
                        .Include(x => x.ShowTime)
                        .ThenInclude(x => x.CinemaRooms)
                        .Where(x => !x.DelFlag && x.DateEnd >= DateTimeOffset.Now
                            //Phim đang chiếu
                            &&
                            (
                                (x.DateStart <= startOfDay
                                && x.DateEnd >= startOfDay)
                                || (
                                    x.DateStartPostpone <= startOfDay
                                    && x.DateExtend >= startOfDay
                                )
                                || (
                                    x.DatePostpone == null
                                    && x.DateExtend >= startOfDay
                                )
                            )
                            && (
                                x.DatePostpone == null
                                || x.DatePostpone > startOfDay
                            )
                        )
                        .Select(
                            x => new ShowTimeByDate()
                            {
                                filmInfo = new FilmInfo()
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Actor = x.Actor,
                                    Director = x.Director,
                                    AgeLimit = x.AgeLimit,
                                    Time = x.Time,
                                    Introduce = x.Introduce,
                                    TrailerLink = x.TrailerLink,
                                    Country = x.Country,
                                    BackgroundImage = x.BackgroundImage,
                                    DateStart = x.DateStart,
                                    DateEnd = x.DateEnd,
                                    Status = x.Status,
                                    Language = x.Language
                                },
                                showTimeDetailByDates = x.ShowTime
                                    .Where(x => !x.DelFlag
                                        && x.DateShow >= from
                                        && x.DateShow <= endOfDaySearch
                                        && x.BranchId == searchCondition.BranchId
                                        && (
                                            searchCondition.CinemeRoomId == null
                                            || x.CinemaRoomId == searchCondition.CinemeRoomId
                                        )
                                        && !x.CinemaRooms.DelFlag
                                        && ((x.FromHour * 60 + x.FromMinus) > now || DateTime.Now.Date != searchCondition.DateRecord.Value.Date))
                                    .Select(x => new ShowTimeDetailByDate()
                                    {
                                        FromHour = x.FromHour,
                                        ToHour = x.ToHour,
                                        FromMinus = x.FromMinus,
                                        ToMinus = x.ToMinus,
                                        Id = x.Id
                                    }).ToList()
                            }
                        );
                    if (searchCondition.CinemeRoomId != null)
                    {
                        showTimeByDates = showTimeByDates.Where(x => x.showTimeDetailByDates.Any());
                    }
                    var totalCount = showTimeByDates.Count();
                    if (totalCount == 0)
                    {
                        continue;
                    }

                    listShowTimeByDate.Paging = new Paging(totalCount, searchCondition.CurrentPage, searchCondition.PageSize);
                    listShowTimeByDate.showTimeByDates = await showTimeByDates.Skip((searchCondition.CurrentPage - 1) * searchCondition.PageSize)
                                                    .Take(searchCondition.PageSize)
                                                    .ToListAsync();
                    listShowTimeByDateReturn.Add(listShowTimeByDate);
                }
                return listShowTimeByDateReturn;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
    }
}

