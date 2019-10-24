using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Weather.Models;

namespace Weather.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        public ActionResult Index()
        {
            var list = from loc in db.Localizations select new WeatherViewModel { City = loc.Name, Weather = loc.DailyWeathers.FirstOrDefault() };

            return View(list);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Localization localization = db.Localizations.FirstOrDefault(l => l.Name == id);
            ViewBag.Title = localization.Name;
            if (localization == null)
            {
                return HttpNotFound();
            }

            return View(localization);
        }
    }
}