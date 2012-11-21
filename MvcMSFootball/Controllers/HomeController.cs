using System.Web.Mvc;

namespace MvcMSFootball.Controllers
{
    /// <summary>
    /// The Home controller class
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Index of the application
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Making Sense Footbal Manager";

            return View();
        }

        /// <summary>
        /// About view
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Redirects to the teams page
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult Teams()
        {
            return RedirectToAction("Index", "Teams");
        }
    }
}
