using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Weather.Models
{
    [Table("Localizations")]
    public class Localization
    {
        [Key]
        public int LocalizationId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdate { get; set; }
        public virtual ICollection<DailyWeather> DailyWeathers { get; set; }

        public Localization()
        { 
        }
    }
}