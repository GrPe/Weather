using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Weather.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
           : base("DefaultConnection")
        {
        }

        public virtual DbSet<DailyWeather> DailyWeather { get; set; }
        public virtual DbSet<Localization> Localizations { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public IEnumerable<WeatherViewModel> GetActualWeatherForAllLocalizations()
        {
            var list = from loc in Localizations select new WeatherViewModel { City = loc.Name, Weather = loc.DailyWeathers.OrderBy(w => w.Time).FirstOrDefault() };

            return list;
        }

        public Localization GetLocalization(string name)
        {
            return Localizations.FirstOrDefault(l => l.Name == name);
        }

        public void UpdateForecastForLocalization(string locName, DarkSky.Models.Forecast forecast)
        {
            Localization localization = GetLocalization(locName);

            if (localization == null)
                return;

            DailyWeather.RemoveRange(localization.DailyWeathers);

            foreach(var day in forecast.Daily.Data)
            {
                DailyWeather.Add(new Models.DailyWeather(day, localization.LocalizationId));
            }
            localization.LastUpdate = DateTime.Now;

            SaveChanges();
        }
    }

    public static class DatabaseInitializer
    {
        public static void Init(ApplicationDbContext context)
        {
            if (context.Localizations.Count() != 0)
                return;

            Localization lublin = new Localization { Name = "Lublin", Latitude = 51.246452, Longitude = 22.568445, LastUpdate = new DateTime(2019, 10, 10) };
            context.Localizations.Add(lublin);
            context.SaveChanges();

            lublin = context.Localizations.FirstOrDefault(l => l.Name == "Lublin");

            var weatherLublin = new List<DailyWeather>()
            {
                new DailyWeather { Icon = "ClearDay", Time = new DateTime(2019, 10, 10), TemperatureHigh = 15 , LocalizationId = lublin.LocalizationId},
                new DailyWeather { Icon = "ClearNight", Time = new DateTime(2019, 10, 11), TemperatureHigh = 0 , LocalizationId = lublin.LocalizationId},
                new DailyWeather { Icon = "Rain", Time = new DateTime(2019, 10, 12), TemperatureHigh = 20 , LocalizationId = lublin.LocalizationId},
                new DailyWeather { Icon = "Snow", Time = new DateTime(2019, 10, 13), TemperatureHigh = 14 , LocalizationId = lublin.LocalizationId},
                new DailyWeather { Icon = "Sleet", Time = new DateTime(2019, 10, 14), TemperatureHigh = 24 , LocalizationId = lublin.LocalizationId},
                new DailyWeather { Icon = "Wind", Time = new DateTime(2019, 10, 15), TemperatureHigh = 19 , LocalizationId = lublin.LocalizationId},
                new DailyWeather { Icon = "Fog", Time = new DateTime(2019, 10, 16), TemperatureHigh = 18 , LocalizationId = lublin.LocalizationId}
            };
            context.DailyWeather.AddRange(weatherLublin);

            context.SaveChanges();

            Localization newyork = new Localization { Name = "Nowy York", Latitude = 40.730610, Longitude = -73.935242, LastUpdate = new DateTime(2019, 10, 10) };
            context.Localizations.Add(newyork);
            context.SaveChanges();

            newyork = context.Localizations.FirstOrDefault(l => l.Name == "Nowy York");

            var weatherNewYork = new List<DailyWeather>()
            {
                new DailyWeather { Icon = "Cloudy", Time = new DateTime(2019, 10, 10), TemperatureHigh = 20 , LocalizationId = newyork.LocalizationId},
                new DailyWeather { Icon = "PartlyCloudyNight", Time = new DateTime(2019, 10, 11), TemperatureHigh = 20 , LocalizationId = newyork.LocalizationId},
                new DailyWeather { Icon = "Cloudy", Time = new DateTime(2019, 10, 12), TemperatureHigh = 20 , LocalizationId = newyork.LocalizationId}
            };

            context.DailyWeather.AddRange(weatherNewYork);
            context.SaveChanges();

            Localization tokyo = new Localization { Name = "Tokio", Latitude = 35.652832, Longitude = 139.839478, LastUpdate = new DateTime(2019, 10, 10) };
            context.Localizations.Add(tokyo);
            context.SaveChanges();

            tokyo = context.Localizations.FirstOrDefault(l => l.Name == "Tokio");

            var weatherTokyo = new List<DailyWeather>()
            {
                new DailyWeather { Icon = "PartlyCloudyDay", Time = new DateTime(2019, 10, 10), TemperatureHigh = 20 , LocalizationId = tokyo.LocalizationId},
                new DailyWeather { Icon = "Cloudy", Time = new DateTime(2019, 10, 11), TemperatureHigh = 20 , LocalizationId = tokyo.LocalizationId},
                new DailyWeather { Icon = "Cloudy", Time = new DateTime(2019, 10, 12), TemperatureHigh = 20 , LocalizationId = tokyo.LocalizationId}
            };
            context.DailyWeather.AddRange(weatherTokyo);

            context.SaveChanges();
        }
    }

}