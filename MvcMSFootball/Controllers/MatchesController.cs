using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using MvcMSFootball.Models;

namespace MvcMSFootball.Controllers
{
    /// <summary>
    /// The Matches controller class
    /// </summary>
    public class MatchesController : Controller
    {

        /// <summary>
        /// Generates a list for the team select filter
        /// </summary>
        private void GenerateTeamSelect()
        {
            using (var context = new FootballManagerDBContext())
            {
                var teams = context.Teams.ToList();
                var teamSelectListItems = teams.Select(team => new SelectListItem() { Text = team.Name, Value = "" + team.ID }).ToList();
                ViewBag.teamListHome = teamSelectListItems;
                ViewBag.teamListAway = teamSelectListItems;
            }
        }

        //
        // GET: /Matches/
        /// <summary>
        /// Sends the match list to the view
        /// </summary>
        /// <returns>The view with the match list</returns>
        public ViewResult Index()
        {
            using (var context = new FootballManagerDBContext())
            {
                return View(context.Matches.ToList());
            }
        }

        //
        // GET: /Matches/Details/5
        /// <summary>
        /// Sends the match details to the view
        /// </summary>
        /// <param name="id">The selected match id</param>        
        /// <returns>The view</returns>
        public ViewResult Details(int id)
        {
            Match match;
            using (var context = new FootballManagerDBContext())
            {
                match = context.Matches.Find(id);
            }
            return View(match);
        }

        //
        // GET: /Matches/Create
        /// <summary>
        /// Generates the teams of the select box and returns the create match view
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult Create()
        {

            // use model in all places where is possible to do it
            GenerateTeamSelect();
            return View();
        }

        //
        // POST: /Matches/Create
        /// <summary>
        /// Creates the Match
        /// </summary>
        /// <param name="match">The match to be created</param>
        /// /// <param name="teamListHome">The home team</param>
        /// /// <param name="teamListAway">The away team</param>
        /// <returns>If the model is valid, it redirects to Index</returns>
        [HttpPost]
        public ActionResult Create(Match match, string teamListHome, string teamListAway)
        {
            var teamHomeId = Convert.ToInt32(teamListHome);
            var teamAwayId = Convert.ToInt32(teamListAway);
            if (ModelState.IsValid)
            {
                using (var context = new FootballManagerDBContext())
                {
                    Team teamHome = context.Teams.Single(d => d.ID == teamHomeId);
                    Team teamAway = context.Teams.Single(d => d.ID == teamAwayId);
                    match.HomeTeamId = teamHomeId;
                    match.AwayTeamId = teamAwayId;
                    match.HomeTeamName = teamHome.Name;
                    match.AwayTeamName = teamAway.Name;
                    context.Matches.Add(match);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(match);
        }

        //
        // GET: /Matches/Edit/5
        /// <summary>
        /// Sends the match details to the edit view 
        /// </summary>
        /// <param name="id">The selected match id</param>        
        /// <returns>The view</returns>
        public ActionResult Edit(int id)
        {
            Match match;
            using (var context = new FootballManagerDBContext())
            {
                match = context.Matches.Find(id);
            }
            return View(match);
        }

        //
        // POST: /Matches/Edit/5
        /// <summary>
        /// Saves the Match edited data
        /// </summary>
        /// <param name="match">The match to be created</param>     
        /// <returns>If the model is valid, it redirects to Index</returns>
        [HttpPost]
        public ActionResult Edit(Match match)
        {
            if (ModelState.IsValid)
            {
                using (var context = new FootballManagerDBContext())
                {
                    context.Entry(match).State = EntityState.Modified;
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View(match);
        }

        //
        // GET: /Matches/Delete/5
        /// <summary>
        /// Sends the match details to the deelete view 
        /// </summary>
        /// <param name="id">The selected match id</param>        
        /// <returns>The view</returns>
        public ActionResult Delete(int id)
        {
            Match match;
            using (var context = new FootballManagerDBContext())
            {
                match = context.Matches.Find(id);
            }
            return View(match);
        }

        //
        // POST: /Matches/Delete/5
        /// <summary>
        /// Deletes the match
        /// </summary>
        /// <param name="match">The match id to be deleted</param>     
        /// <returns>If succeeds it redirects to Index</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var context = new FootballManagerDBContext())
            {
                Match match = context.Matches.Find(id);
                context.Matches.Remove(match);
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