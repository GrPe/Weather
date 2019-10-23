using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Weather.Models
{
    public class DailyWeather
    {
        [Key]
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Icon { get; set; }
        public DateTime Time { get; set; }
        public double TemperatureHigh { get; set; }
        public double TemperatureLow { get; set; }
        public double Pressure { get; set; }
        public int UVIndex { get; set; }
        public Localization Localization { get; set; }

        public DailyWeather()
        {

        }

        public DailyWeather(DarkSky.Models.Forecast forecast, int dayIndex)
        {
            Summary = forecast.Daily.Data[dayIndex].Summary;
            Icon = forecast.Daily.Data[dayIndex].Icon.ToString();
            Time = forecast.Daily.Data[dayIndex].DateTime.DateTime;
            TemperatureHigh = forecast.Daily.Data[dayIndex].TemperatureHigh ?? 0;
            TemperatureLow = forecast.Daily.Data[dayIndex].TemperatureLow ?? 0;
            Pressure = forecast.Daily.Data[dayIndex].Pressure ?? 0;
            UVIndex = forecast.Daily.Data[dayIndex].UvIndex ?? 0;
        }
    }
}