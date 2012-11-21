using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MvcMSFootball.Models;

namespace MvcMSFootball.Controllers
{
    /// <summary>
    /// The Players controller
    /// </summary>
    public class PlayersController : Controller
    {

        /// <summary>
        /// Generates a list for the team select filter. If the Player is about to be created, the list populates itself with no order. If 
        /// the player is about to be edited then the select has the actual team already selected
        /// </summary>
        /// <param name="idTeam">The ID of the team to be edited.</param>
        private void GenerateTeamSelect(int idTeam = 0)
        {
            using (var context = new FootballManagerDBContext())
            {
                var teams = context.Teams.ToList();
                var teamSelectListItem = new List<SelectListItem>();
                if (idTeam == 0)
                {
                    teamSelectListItem = teams.Select(team => new SelectListItem() { Text = team.Name, Value = "" + team.ID }).ToList();
                }
                else
                {
                    // If idTeam is passed as an argument it means that the user is editing a player. 
                    // In that case the dropdown should select by default the player's team.

                    var teamShorterList = new List<Team>();
                    // We add the player's team firt
                    Team team = teams.Single(selectedTeam => selectedTeam.ID == idTeam);
                    teamShorterList.Add(team);

                    // Then we populate the list with the rest of the teams
                    teamShorterList.AddRange(teams.Where(teamListItem => teamListItem.ID != idTeam).ToList());

                    teamSelectListItem = teamShorterList.Select(selectedTeam => new SelectListItem() { Text = selectedTeam.Name, Value = "" + selectedTeam.ID }).ToList();
                }
                ViewBag.teamList = teamSelectListItem;
            }
        }

        /// <summary>
        /// Filters the list of players
        /// </summary>
        /// <param name="teamList">The team selected id</param>
        /// <param name="searchString">The string to use as filter</param>
        /// <returns>The view</returns>
        public ActionResult IndexList(String teamList, String searchString)
        {

            GenerateTeamSelect();

            List<Player> playersList;

            using (var context = new FootballManagerDBContext())
            {

                var players = from p in context.Players.Include(p => p.Team) select p;

                if (!String.IsNullOrEmpty(teamList))
                {
                    var teamId = Int32.Parse(teamList);
                    players = players.Where(player => player.TeamId == teamId);
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    players = players.Where(player => player.Name.Contains(searchString));
                }

                playersList = players.OrderByDescending(player => player.Rating).ToList();

                return View(playersList);
            }

        }

        //
        // GET: /Players/Details/5
        /// <summary>
        /// Details view controller method for showing player details
        /// </summary>
        /// <param name="id">The player id</param>
        /// <returns>The Details view</returns>
        public ViewResult Details(int id)
        {

            Player player;
            using (var context = new FootballManagerDBContext())
            {
                player = context.Players.Find(id);
            }
            return View(player);
        }

        //
        // GET: /Players/Create
        /// <summary>
        /// Create view controller method for creating a player
        /// </summary>
        /// <returns>The Create view</returns>
        public ActionResult Create()
        {
            GenerateTeamSelect();
            return View();
        }

        //
        // POST: /Players/Create
        /// <summary>
        /// Creates the player
        /// </summary>
        /// <returns>If it's valid, redirects to indexList</returns>
        [HttpPost]
        public ActionResult Create(Player player, String teamList)
        {
            var teamId = Convert.ToInt32(teamList);
            if (ModelState.IsValid)
            {
                using (var context = new FootballManagerDBContext())
                {
                    Team team = context.Teams.Single(selectedTeam => selectedTeam.ID == teamId);
                    player.Team = team;
                    player.TeamId = team.ID;
                    context.Players.Add(player);
                    context.SaveChanges();
                }
                return RedirectToAction("IndexList");
            }

            return View(player);
        }

        //
        // GET: /Players/Edit/5
        /// <summary>
        /// Edit view controller method for editing a player
        /// </summary>
        /// <param name="id">The player id</param>
        /// <returns>The Edit view</returns>
        public ActionResult Edit(int id)
        {
            Player player;
            using (var context = new FootballManagerDBContext())
            {
                player = context.Players.Find(id);
            }
            GenerateTeamSelect(player.TeamId);
            return View(player);
        }

        //
        // POST: /Players/Edit/5
        /// <summary>
        /// Saves the edited player data
        /// </summary>
        /// <returns>If it's valid, redirects to indexList</returns>
        [HttpPost]
        public ActionResult Edit(Player player, String teamList)
        {
            var teamId = Convert.ToInt32(teamList);
            if (ModelState.IsValid)
            {
                using (var context = new FootballManagerDBContext())
                {
                    Team team = context.Teams.Single(selectedTeam => selectedTeam.ID == teamId);
                    player.TeamId = teamId;
                    player.Team = team;
                    context.Entry(player).State = EntityState.Modified;
                    context.SaveChanges();
                }
                return RedirectToAction("IndexList");
            }
            return View(player);
        }

        //
        // GET: /Players/Delete/5
        /// <summary>
        /// Delete view controller method for deleting a player
        /// </summary>
        /// <param name="id">The player id</param>
        /// <returns>The Delete view</returns>
        public ActionResult Delete(int id)
        {
            Player player;
            using (var context = new FootballManagerDBContext())
            {
                player =  context.Players.Find(id);
            }
            return View(player);
        }

        //
        // POST: /Players/Delete/5
        /// <summary>
        /// Deletes the player
        /// </summary>
        /// <returns>If it's valid, redirects to indexList</returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player;
            using (var context = new FootballManagerDBContext())
            {
                player = context.Players.Find(id);
                context.Players.Remove(player);
                context.SaveChanges();
            }
            return RedirectToAction("IndexList");
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