using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Weather.Models
{
    [Table("DailyWeathers")]
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
        public double Wind { get; set; }

        [ForeignKey("Localization")]
        public int LocalizationId { get; set; }
        public virtual Localization Localization { get; set; }

        public DailyWeather()
        {

        }

        public DailyWeather(DarkSky.Models.DataPoint forecast, int locId)
        {
            Summary = forecast.Summary;
            Icon = forecast.Icon.ToString();
            Time = forecast.DateTime.DateTime;
            TemperatureHigh = forecast.TemperatureHigh ?? 0;
            TemperatureLow = forecast.TemperatureLow ?? 0;
            Pressure = forecast.Pressure ?? 0;
            Wind = forecast.WindSpeed ?? 0;
            LocalizationId = locId;
        }
    }
}