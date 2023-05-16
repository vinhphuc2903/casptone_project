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
using CapstoneProject.Areas.Film.Models.FilmAdminModels.Schemas;

namespace CapstoneProject.Areas.Film.Models.FilmAdminModels
{
    public interface IFilmAdminModels
    {
        //Task<ListFilms> GetListFilms(SearchCondition searchCondition);
        Task<ResponseInfo> AddNewFilmData(NewFilmData newFilmData);
    }
    public class FilmAdminModel : CapstoneProjectModels, IFilmAdminModels
    {
        private readonly ILogger<FilmAdminModel> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";

        public FilmAdminModel(
            IConfiguration configuration,
            ILogger<FilmAdminModel> logger,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        //public async Task<ListFilms> GetListFilms(SearchCondition searchCondition)
        //{
        //    string method = GetActualAsyncMethodName();
        //    IDbContextTransaction transaction = null;
        //    try
        //    {
        //        ListFilms listFilms = new ListFilms();
        //        listFilms.ListFilmInfos = await _context.Films.Where(x => !x.DelFlag)
        //                                                .Select(x => new FilmInfo()
        //                                                {
        //                                                    Id = x.Id,
        //                                                    Name = x.Name,
        //                                                    Actor = x.Actor,
        //                                                    Director = x.Director,
        //                                                    Age = x.AgeLimit,
        //                                                    Time = x.Time,
        //                                                }
        //                                                ).ToListAsync();
        //        foreach (var film in listFilms.ListFilmInfos)
        //        {
        //            film.ListTypeFilm = await _context.TypeFilmDetails
        //                                            .Where(x => !x.DelFlag && x.FilmId == film.Id)
        //                                            .Include(x => x.TypeFilms)
        //                                            .Select(x => x.TypeFilms.Name).ToListAsync();
        //        }
        //        return listFilms;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation($"Get List Film Error: {ex}");
        //        throw ex;
        //    }
        //}
        public async Task<ResponseInfo> AddNewFilmData(NewFilmData newFilmData)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            ResponseInfo responseInfo = new ResponseInfo();
            try
            {
                //var film = await _context.Films.Where(x => !x.DelFlag && x.Name.Contains(newFilmData.FilmData.Name)).FirstOrDefaultAsync();
                FilmData typefilm = new FilmData()
                {
                    Name = newFilmData.Name,
                    Actor = newFilmData.Actor,
                    Director = newFilmData.Director,
                    AgeLimit = newFilmData.AgeLimit,
                    Time = newFilmData.Time,
                    Introduce = newFilmData.Introduce,
                    TrailerLink = newFilmData.TrailerLink,
                    Country = newFilmData.Country,
                    BackgroundImage = newFilmData.BackgroundImage,
                    DateStart = newFilmData.DateStart,
                    DateEnd = newFilmData.DateEnd,
                    Status = newFilmData.Status,
                    Language = newFilmData.Language
                };
                await _context.Films.AddAsync(typefilm);
                await _context.SaveChangesAsync();
                var listType = newFilmData.ListTypeFilm.Trim().Replace(" ", "").Split(',');
                foreach (var type in listType)
                {
                    TypeFilmData typeFilm = new TypeFilmData()
                    {
                        FilmId = typefilm.Id,
                        TypeFilmId = Int32.Parse(type)
                    };
                    await _context.TypeFilmDetails.AddAsync(typeFilm);
                    await _context.SaveChangesAsync();

                }
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

