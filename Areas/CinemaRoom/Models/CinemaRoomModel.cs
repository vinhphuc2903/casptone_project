using System;
using CapstoneProject.Commons.Schemas;
using CapstoneProject.Areas.CinemaRoom.Models.Schemas;
using CapstoneProject.Databases.Schemas.System.Film;
using CapstoneProject.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using BranchData = CapstoneProject.Databases.Schemas.Setting.Branch;
using CinemaRoomDb = CapstoneProject.Databases.Schemas.System.CinemaRoom.CinemaRooms;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Commons.Enum;
using SeatDb = CapstoneProject.Databases.Schemas.System.CinemaRoom.Seat;
using CapstoneProject.Commons.CodeMaster;
using CapstoneProject.Services;

namespace CapstoneProject.Areas.CinemaRoom.Models
{
	public interface ICinemaRoomModel
	{
		Task<List<CinemaRoomData>> GetAllCinemaRoom(SearchCondition searchCondition);
        Task<List<BranchData>> GetAllBranches(SearchCondition searchCondition);
        Task<ResponseInfo> UpdateCinemaRoomData(CinemaRoomDataInput cinemaRoomData);
        Task<ResponseInfo> CreateCinemaRoomData(CinemaRoomDataInput cinemaRoomData);
        Task<ResponseInfo> DeleteCinemaRoomData(int id);
        Task<List<CinemaRoomDataInput>> GetAllCinemaRoomData(SearchCondition searchCondition);
    }

    public class CinemaRoomModel : CapstoneProjectModels, ICinemaRoomModel
    {
        private readonly ILogger<CinemaRoomData> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        private readonly IIdentityService _indentityService;

        public CinemaRoomModel(
            IConfiguration configuration,
            ILogger<CinemaRoomData> logger,
            IServiceProvider provider,
            IIdentityService identityService
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _indentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<List<CinemaRoomDataInput>> GetAllCinemaRoomData(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                List<CinemaRoomDataInput> list = _context.CinemaRooms
                    .Include(x => x.Branches)
                    .Where(x => !x.DelFlag
                        && (searchCondition.BranchId == null
                            || x.BranchId == searchCondition.BranchId
                        )
                    )
                    .Select(x => new CinemaRoomDataInput
                    {
                        Name = x.Name,
                        Id = x.Id,
                        BranchId = x.BranchId,
                        TotalColumn = x.TotalColumn,
                        TotalSeat = x.TotalSeat,
                        TotalRow = x.TotalRow,
                        BranchName = x.Branches.Name,
                    })
                    .ToList();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<List<CinemaRoomData>> GetAllCinemaRoom(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                List<CinemaRoomData> list = _context.CinemaRooms
                    .Where(x => !x.DelFlag
                        && (searchCondition.BranchId == null
                            || x.BranchId == searchCondition.BranchId
                        )
                    )
                    .Select(x => new CinemaRoomData{
                        Name = x.Name,
                        Id = x.Id
                    })
                    .ToList();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<List<BranchData>> GetAllBranches(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                List<BranchData> list = _context.Branches
                    .Where(x => !x.DelFlag
                        && (searchCondition.BranchId == null
                            || x.Id == searchCondition.BranchId
                        )
                    )
                    .ToList();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ResponseInfo> UpdateCinemaRoomData(CinemaRoomDataInput cinemaRoomData)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                ResponseInfo responseInfo = new ResponseInfo();
                if (!await _indentityService.CheckIndentifyUser(R001.ADMIN.CODE))
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.ACCOUNT_NOT_HAVE_PERMISSION;
                    return responseInfo;
                }
                var cinemaCheck = _context.CinemaRooms.Where(x => x.Id == cinemaRoomData.Id && !x.DelFlag).FirstOrDefault();
                if(cinemaCheck != null)
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.NAME_IS_EXITED;
                    return responseInfo;
                }
                cinemaCheck.Name = cinemaRoomData.Name;
                cinemaCheck.TotalColumn = cinemaRoomData.TotalColumn;
                cinemaCheck.TotalRow = cinemaRoomData.TotalRow;
                cinemaCheck.TotalColumn = cinemaRoomData.TotalColumn * cinemaRoomData.TotalRow;
                cinemaCheck.BranchId = cinemaRoomData.BranchId;
                List<SeatDb> listSeatOld = await _context.Seats.Where(x => !x.DelFlag && x.CinemaRoomId == cinemaRoomData.Id).ToListAsync();
                foreach(var seat in listSeatOld)
                {
                    seat.DelFlag = true;
                }    
                _context.SaveChanges();
                List<SeatDb> seats = new List<SeatDb>();
                for(int index = 0; index < cinemaRoomData.TotalRow; index++)
                {
                    char columnChar = (char)('A' + index);
                    for (int col = 0; col < cinemaRoomData.TotalColumn; col++)
                    {
                        seats.Add(new SeatDb()
                        {
                            CinemaRoomId = cinemaCheck.Id,
                            Type = 10,
                            SeatCode = columnChar + (col + 1).ToString()
                        });
                    }    
                }
                _context.Seats.AddRange(seats);
                _context.SaveChanges();
                return responseInfo;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ResponseInfo> CreateCinemaRoomData(CinemaRoomDataInput cinemaRoomData)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                ResponseInfo responseInfo = new ResponseInfo();
                if (!await _indentityService.CheckIndentifyUser(R001.ADMIN.CODE))
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.ACCOUNT_NOT_HAVE_PERMISSION;
                    return responseInfo;
                }
                var cinemaCheck = _context.CinemaRooms.Where(x => x.Id == cinemaRoomData.Id && !x.DelFlag && x.BranchId == cinemaRoomData.BranchId).FirstOrDefault();
                if (cinemaCheck != null)
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.NAME_IS_EXITED;
                    return responseInfo;
                }
                CinemaRoomDb cinemaRoomDb = new CinemaRoomDb()
                {
                    Name = cinemaRoomData.Name,
                    TotalColumn = cinemaRoomData.TotalColumn,
                    TotalRow = cinemaRoomData.TotalRow,
                    TotalSeat = cinemaRoomData.TotalColumn * cinemaRoomData.TotalRow,
                    BranchId = cinemaRoomData.BranchId,
                };
                await _context.CinemaRooms.AddAsync(cinemaRoomDb);
                _context.SaveChanges();
                List<SeatDb> seats = new List<SeatDb>();
                for (int index = 0; index < cinemaRoomData.TotalRow; index++)
                {
                    char columnChar = (char)('A' + index);
                    for (int col = 0; col < cinemaRoomData.TotalColumn; col++)
                    {
                        seats.Add(new SeatDb()
                        {
                            CinemaRoomId = cinemaRoomDb.Id,
                            Type = 10,
                            SeatCode = columnChar + (col + 1).ToString()
                        });
                    }
                }
                _context.Seats.AddRange(seats);
                _context.SaveChanges();
                return responseInfo;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ResponseInfo> DeleteCinemaRoomData(int id)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                ResponseInfo responseInfo = new ResponseInfo();
                if (!await _indentityService.CheckIndentifyUser(R001.ADMIN.CODE))
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.ACCOUNT_NOT_HAVE_PERMISSION;
                    return responseInfo;
                }
                CinemaRoomDb cinemaCheck = _context.CinemaRooms.Where(x => x.Id == id && !x.DelFlag).FirstOrDefault();
                if (cinemaCheck == null)
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.ROOM_IS_NOT_EXITED;
                    return responseInfo;
                }
                cinemaCheck.DelFlag = true;
                List<SeatDb> seats = _context.Seats.Where(x => x.CinemaRoomId == id && x.DelFlag == false).ToList();
                foreach(var seat in seats)
                {
                    seat.DelFlag = true;
                }    
                _context.SaveChanges();
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

