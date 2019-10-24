using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Weather.Models;

namespace Weather.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = ApplicationDbContext.Create();
        private string apiKey = "25e71403e9e869e1e562f82c2d2e3101";

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.GetActualWeatherForAllLocalizations());
        }

        [HttpGet]
        public ActionResult UpdateAll()
        {
            return View();
        }

        [HttpPost, ActionName("UpdateAll")]
        public async Task<ActionResult> ConfirmUpdateAll()
        {
            DarkSky.Services.DarkSkyService darkSkyService = new DarkSky.Services.DarkSkyService(apiKey);
            DarkSky.Models.OptionalParameters parameters = new DarkSky.Models.OptionalParameters()
            {
                MeasurementUnits = "si",
                LanguageCode = "pl"
            };

            foreach (var loc in db.Localizations)
            {
                var forecast = await darkSkyService.GetForecast(loc.Latitude, loc.Longitude, parameters);

                if(forecast.IsSuccessStatus == true)
                {
                    db.UpdateForecastForLocalization(loc.Name, forecast.Response);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Update(string id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Localization localization = db.GetLocalization(id);

            if (localization == null)
            {
                return HttpNotFound();
            }

            return View(localization);
        }

        [HttpPost, ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateConfirm(string id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Localization localization = db.Localizations.FirstOrDefault(l => l.Name == id);

            if(localization == null)
            {
                return HttpNotFound();
            }

            DarkSky.Services.DarkSkyService darkSkyService = new DarkSky.Services.DarkSkyService(apiKey);
            DarkSky.Models.OptionalParameters parameters = new DarkSky.Models.OptionalParameters()
            {
                MeasurementUnits = "si",
                LanguageCode ="pl"
            };
            
            var forecast = await darkSkyService.GetForecast(localization.Latitude, localization.Longitude,parameters);

            if(forecast?.IsSuccessStatus == true)
            {
                db.UpdateForecastForLocalization(id, forecast.Response);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}