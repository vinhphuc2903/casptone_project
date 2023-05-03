using System;
using CapstoneProject.Areas.WeatherForecast.Models.Schemas;
using Microsoft.AspNetCore.Mvc;
using System;
using CapstoneProject.Controllers;

namespace CapstoneProject.Areas.WeatherForecast.Controllers
{
    [Area("Weather")]
    public class WeatherAreaController : BaseController
    {
        public WeatherAreaController(IServiceProvider provider) : base(provider)
        {

        }
    }
}

