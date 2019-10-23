using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DarkSky.Models;
using Microsoft.AspNet.Identity.EntityFramework;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Localization>()
                .HasMany(l => l.DailyWeathers)
                .WithRequired(w => w.Localization)
                .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
    }

    public static class DatabaseInitializer
    {
        public static void Init(ApplicationDbContext context)
        {
            Localization lublin = new Localization { Name = "Lublin", Latitude = 51.246452, Longitude = 22.568445, LastUpdate = new DateTime(2019, 10, 10) };
            var weatherLublin = new List<DailyWeather>()
            {
                //City = "Lublin", Icon = "cloudy", Latitude = 51.246452, Longitude = 22.568445
                new DailyWeather { Icon = "cloudy", Time = new DateTime(2019, 10, 10), TemperatureHigh = 20 , Localization = lublin},
                new DailyWeather { Icon = "cloudy", Time = new DateTime(2019, 10, 11), TemperatureHigh = 20 , Localization = lublin},
                new DailyWeather { Icon = "cloudy", Time = new DateTime(2019, 10, 12), TemperatureHigh = 20 , Localization = lublin}
            };
            context.DailyWeather.AddRange(weatherLublin);
            lublin.DailyWeathers.Add(weatherLublin[0]);
            lublin.DailyWeathers.Add(weatherLublin[1]);
            lublin.DailyWeathers.Add(weatherLublin[2]);
            context.Localizations.Add(lublin);


            Localization newyork = new Localization { Name = "New York", Latitude = 40.730610, Longitude = -73.935242, LastUpdate = new DateTime(2019, 10, 10) };
            var weatherNewYork = new List<DailyWeather>()
            {
                //City = "Lublin", Icon = "cloudy", Latitude = 51.246452, Longitude = 22.568445
                new DailyWeather { Icon = "cloudy", Time = new DateTime(2019, 10, 10), TemperatureHigh = 20 , Localization = newyork},
                new DailyWeather { Icon = "cloudy", Time = new DateTime(2019, 10, 11), TemperatureHigh = 20 , Localization = newyork},
                new DailyWeather { Icon = "cloudy", Time = new DateTime(2019, 10, 12), TemperatureHigh = 20 , Localization = newyork}
            };
            context.DailyWeather.AddRange(weatherNewYork);
            newyork.DailyWeathers.Add(weatherNewYork[0]);
            newyork.DailyWeathers.Add(weatherNewYork[1]);
            newyork.DailyWeathers.Add(weatherNewYork[2]);
            context.Localizations.Add(newyork);

            Localization tokyo = new Localization { Name = "New York", Latitude = 35.652832, Longitude = 139.839478, LastUpdate = new DateTime(2019, 10, 10) };
            var weatherTokyo = new List<DailyWeather>()
            {
                //City = "Lublin", Icon = "cloudy", Latitude = 51.246452, Longitude = 22.568445
                new DailyWeather { Icon = "cloudy", Time = new DateTime(2019, 10, 10), TemperatureHigh = 20 , Localization = tokyo},
                new DailyWeather { Icon = "cloudy", Time = new DateTime(2019, 10, 11), TemperatureHigh = 20 , Localization = tokyo},
                new DailyWeather { Icon = "cloudy", Time = new DateTime(2019, 10, 12), TemperatureHigh = 20 , Localization = tokyo}
            };
            context.DailyWeather.AddRange(weatherTokyo);
            tokyo.DailyWeathers.Add(weatherTokyo[0]);
            tokyo.DailyWeathers.Add(weatherTokyo[1]);
            tokyo.DailyWeathers.Add(weatherTokyo[2]);
            context.Localizations.Add(tokyo);


            context.SaveChanges();
        }
    }

}