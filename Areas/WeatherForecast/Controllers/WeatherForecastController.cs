using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Areas.WeatherForecast.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.WeatherForecast.Models.Schemas;

namespace CapstoneProject.Areas.WeatherForecast.Controllers
{
    [Route(("api/{api_version:apiVersion}/weather"))]
    public class WeatherForecastController : WeatherAreaController
    {
        private readonly IWeatherForecastModel _weatherForecastModel;
        public WeatherForecastController (
             IWeatherForecastModel weatherForecastModel,
             IServiceProvider provider
        ) : base(provider)
        {
            _weatherForecastModel = weatherForecastModel ?? throw new ArgumentNullException(nameof(weatherForecastModel));
        }
        /// <summary>
        /// Data master sắp xếp theo doanh thu
        /// <para>Created at: 28/12/2022</para>
        /// <para>Created by: TruongNQ</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("all-weather")]
        public async Task<ActionResult> Get([FromQuery]SearchConditon searchConditon)
        {
            try
            {
                return Ok(await _weatherForecastModel.Get(searchConditon));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Data master sắp xếp theo doanh thu
        /// <para>Created at: 28/12/2022</para>
        /// <para>Created by: TruongNQ</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("all-employee")]
        public async Task<ActionResult> GetEmployee()
        {
            try
            {
                return Ok(await _weatherForecastModel.GetEmployees());
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
    }
}