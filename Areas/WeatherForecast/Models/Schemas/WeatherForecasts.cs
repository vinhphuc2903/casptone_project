using System.Collections.Generic;
using System;

namespace CapstoneProject.Areas.WeatherForecast.Models.Schemas
{
    public class WeatherForecasts
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}


