using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.Models
{
    public class WeatherViewModel
    {
        public string City { get; set; }
        public DailyWeather Weather { get; set; }
    }
}