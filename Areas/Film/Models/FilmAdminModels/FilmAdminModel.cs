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
using CapstoneProject.Services;
using CapstoneProject.Commons.CodeMaster;
using Azure;
using CapstoneProject.Commons.Enum;

namespace CapstoneProject.Areas.Film.Models.FilmAdminModels
{
    public interface IFilmAdminModels
    {
        //Task<ListFilms> GetListFilms(SearchCondition searchCondition);
        Task<ResponseInfo> AddNewFilmData(NewFilmData newFilmData);
        Task<ResponseInfo> UpdateFilmData(NewFilmData newFilmData);
        Task<NewFilmData> GetDetailFilm(int id);
    }
    public class FilmAdminModel : CapstoneProjectModels, IFilmAdminModels
    {
        private readonly ILogger<FilmAdminModel> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        private readonly IIdentityService _indentityService;
        private readonly IMediaService _iIMediaService;

        public FilmAdminModel(
            IConfiguration configuration,
            ILogger<FilmAdminModel> logger,
            IServiceProvider provider,
            IIdentityService identityService,
            IMediaService mediaService
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _indentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _iIMediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
        }
        public async Task<NewFilmData> GetDetailFilm(int id)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                FilmData film = await _context.Films.Include(x => x.TypeFilmDetail).Where(x => !x.DelFlag && x.Id == id).FirstOrDefaultAsync();
                NewFilmData typefilm = new NewFilmData()
                {
                    Id = film.Id,
                    Name = film.Name,
                    Actor = film.Actor,
                    Director = film.Director,
                    AgeLimit = film.AgeLimit,
                    Time = film.Time,
                    Introduce = film.Introduce,
                    TrailerLink = film.TrailerLink.Replace("watch?v=", "embed/"),
                    Country = film.Country,
                    BackgroundImageLink = film.BackgroundImage,
                    DateStart = film.DateStart,
                    DateEnd = film.DateEnd,
                    Status = film.Status,
                    Language = film.Language,
                    ListTypeFilmData = film.TypeFilmDetail.Select(x => x.TypeFilmId).ToList()
            };
                return typefilm;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ResponseInfo> AddNewFilmData(NewFilmData newFilmData)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            ResponseInfo responseInfo = new ResponseInfo();
            try
            {
                if(!await _indentityService.CheckIndentifyUser(R001.ADMIN.CODE))
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.ACCOUNT_NOT_HAVE_PERMISSION;
                    return responseInfo;
                }
                //Upload ảnh lên s3
                string linkImage = await _iIMediaService.UploadImageToS3(newFilmData.BackgroundImage, true, 254, 381);
                var film = await _context.Films.Where(x => !x.DelFlag && x.Name.Contains(newFilmData.Name)).FirstOrDefaultAsync();
                FilmData typefilm = new FilmData()
                {
                   Name = newFilmData.Name,
                   Actor = newFilmData.Actor,
                   Director = newFilmData.Director,
                   AgeLimit = newFilmData.AgeLimit,
                   Time = newFilmData.Time,
                   Introduce = newFilmData.Introduce,
                   TrailerLink = newFilmData.TrailerLink.Replace("watch?v=", "embed/"),
                   Country = newFilmData.Country,
                   BackgroundImage = linkImage,
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
                await _context.SaveChangesAsync();
                transaction = await _context.Database.BeginTransactionAsync();
                transaction?.CommitAsync();
                return responseInfo;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ResponseInfo> UpdateFilmData(NewFilmData filmdataUpdate)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            ResponseInfo responseInfo = new ResponseInfo();
            try
            {
                if (!await _indentityService.CheckIndentifyUser(R001.ADMIN.CODE))
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.ACCOUNT_NOT_HAVE_PERMISSION;
                    return responseInfo;
                }
                //var film = await _context.Films.Where(x => !x.DelFlag && x.Name.Contains(filmdataUpdate.Name)).FirstOrDefaultAsync();

                FilmData filmUpdate = await _context.Films.Where(x => !x.DelFlag && x.Id == filmdataUpdate.Id).FirstOrDefaultAsync();
                //Upload ảnh lên s3
                if (filmdataUpdate.BackgroundImage != null)
                {
                    string linkImage = await _iIMediaService.UploadImageToS3(filmdataUpdate.BackgroundImage, true, 254, 381);
                    filmUpdate.BackgroundImage = linkImage;
                }


                //Cap nhat lai film
                filmUpdate.Name = filmdataUpdate.Name;
                filmUpdate.Actor = filmdataUpdate.Actor;
                filmUpdate.Director = filmdataUpdate.Director;
                filmUpdate.AgeLimit = filmdataUpdate.AgeLimit;
                filmUpdate.Time = filmdataUpdate.Time;
                filmUpdate.Introduce = filmdataUpdate.Introduce;
                filmUpdate.TrailerLink = filmdataUpdate.TrailerLink.Replace("watch?v=", "embed/");
                filmUpdate.Country = filmdataUpdate.Country;
                filmUpdate.DateStart = filmdataUpdate.DateStart;
                filmUpdate.DateEnd = filmdataUpdate.DateEnd;
                filmUpdate.Status = filmdataUpdate.Status;
                filmUpdate.Language = filmdataUpdate.Language;
                var listTypeFilmOld = await _context.TypeFilmDetails.Where(x => !x.DelFlag && x.FilmId == filmUpdate.Id).ToListAsync();
                var listType = filmdataUpdate.ListTypeFilm.Trim().Replace(" ", "").Split(',');
                // Xoa typeFilmDetail cu
                foreach (var typeFilm in listTypeFilmOld)
                {
                    _context.TypeFilmDetails.Remove(typeFilm);
                }
                await _context.SaveChangesAsync();
                //Cap nhat lai typeFilm
                foreach (var type in listType)
                {
                    TypeFilmData typeFilm = new TypeFilmData()
                    {
                        FilmId = filmUpdate.Id,
                        TypeFilmId = Int32.Parse(type)
                    };
                    await _context.TypeFilmDetails.AddAsync(typeFilm);
                    await _context.SaveChangesAsync();
                }
                
                await _context.SaveChangesAsync();
                transaction = await _context.Database.BeginTransactionAsync();
                transaction?.CommitAsync();
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

