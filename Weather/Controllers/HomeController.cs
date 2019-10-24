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
            return View(db.GetActualWeatherForAllLocalizations());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Localization localization = db.GetLocalization(id);
            ViewBag.Title = localization.Name;
            if (localization == null)
            {
                return HttpNotFound();
            }

            return View(localization);
        }
    }
}