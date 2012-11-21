using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MvcMSFootball.Models;

namespace MvcMSFootball.Controllers
{
    /// <summary>
    /// The Teams controller class
    /// </summary>
    public class TeamsController : Controller
    {

        //
        // GET: /Teams/
        /// <summary>
        /// Sends to the view the list of teams
        /// </summary>
        /// <returns>The view</returns>
        public ViewResult Index()
        {
            using (var context = new FootballManagerDBContext())
            {
                return View(context.Teams.ToList());
            }
        }

        //
        // GET: /Teams/Details/5
        /// <summary>
        /// Sends to the view layer the data of the team to be edited
        /// </summary>
        /// <param name="id">The team selected id</param>
        /// <returns>The view</returns>
        public ViewResult Details(int id)
        {
            Team team;
            using (var context = new FootballManagerDBContext())
            {

                team = (from t in context.Teams.Include(t => t.Players)
                       where t.ID.Equals(id)
                       select t).FirstOrDefault();
            }
            return View(team);
        }

        //
        // GET: /Teams/Create
        /// <summary>
        /// Create team view
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Teams/Create
        /// <summary>
        /// Creates the team 
        /// </summary>
        /// <param name="team">The team to be created</param>
        /// <returns>If succeeds then it redirects to Index</returns>
        [HttpPost]
        public ActionResult Create(Team team)
        {
            if (ModelState.IsValid)
            {
                using (var context = new FootballManagerDBContext())
                {
                    context.Teams.Add(team);
                    context.SaveChanges();
                }
                return RedirectToAction("Index");  
            }

            return View(team);
        }
        
        //
        // GET: /Teams/Edit/5
        /// <summary>
        /// Sends to the view layer the data of the team to be edited
        /// </summary>
        /// <param name="id">The team selected id</param>        
        /// <returns>The view</returns>
        public ActionResult Edit(int id)
        {
            Team team;
            using (var context = new FootballManagerDBContext())
            {
                 team = context.Teams.Find(id);
            }
            return View(team);
        }

        //
        // POST: /Teams/Edit/5
        /// <summary>
        /// Saves the edited team data in the database
        /// </summary>
        /// <param name="team">The selected team</param>
        /// <returns>If succeeds it redirects to index</returns>
        [HttpPost]
        public ActionResult Edit(Team team)
        {
            if (ModelState.IsValid)
            {
                using (var context = new FootballManagerDBContext())
                {
                    context.Entry(team).State = EntityState.Modified;
                    context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(team);
        }

        //
        // GET: /Teams/Delete/5
        /// <summary>
        /// Sends to the view layer the data of the team to be deleted
        /// </summary>
        /// <param name="id">The team selected id</param>        
        /// <returns>The view</returns>
        public ActionResult Delete(int id)
        {
            Team team;
            using (var context = new FootballManagerDBContext())
            {
                team = context.Teams.Find(id);
            }
            return View(team);
        }

        //
        // POST: /Teams/Delete/5
        /// <summary>
        /// Deletes the team
        /// </summary>
        /// <param name="team">The selected team id</param>
        /// <returns>If succeeds it redirects to index</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var context = new FootballManagerDBContext())
            {
                Team team = context.Teams.Find(id);
                context.Teams.Remove(team);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method in charge of disposing the db context
        /// </summary>
        /// <param name="disposing">indicates if the context should be disposed</param>
        protected override void Dispose(bool disposing)
        {
            using (var context = new FootballManagerDBContext())
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}