using System;
using CapstoneProject.Areas.CinemaRoom.Models.Schemas;
using CapstoneProject.Databases.Schemas.System.Film;
using CapstoneProject.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace CapstoneProject.Areas.CinemaRoom.Models
{
	public interface ICinemaRoomModel
	{
		Task<List<CinemaRoomData>> GetAllCinemaRoom(SearchCondition searchCondition);
	}

    public class CinemaRoomModel : CapstoneProjectModels, ICinemaRoomModel
    {
        private readonly ILogger<CinemaRoomData> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        public CinemaRoomModel(
            IConfiguration configuration,
            ILogger<CinemaRoomData> logger,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
    }
}

