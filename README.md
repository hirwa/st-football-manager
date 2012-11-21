st-football-manager
===================

This self-training application allows the user to create, edit, list and delete players with their respective teams. It also allows match creation with their respective results and date. It allows the user to filter players and to list them by team ( using team details ). 

The objective was to learn the ASP MVC3 framework basics. Before starting this project i completed the Microsoft tutorial ( movie listing ) : http://www.asp.net/mvc/tutorials/getting-started-with-aspnet-mvc3/cs/intro-to-aspnet-mvc-3

Documentation
=============

Microsoft ASP.NET MVC 3 (w/Razor Engine) self training.

Learned topics
--------------

* Model
   * Code First Approach
   * Annotations & validations
* View
   * Razor engine
   * HTML helpers
* Controller
   * Action results, selectors & filters
* Services
   * LINQ
   * ViewBag

This self training objective was to learn the basic concepts of Microsoft's ASP.NET MVC 3 and the Razor View Engine. Also i learned about a TFS.

This particular application lets the user create, edit, delete and list football teams, players and matches. 
Players are assigned to teams, and matches are played by two existing teams.
Also the user wil be able to filter players by name or team.

First of all I created the **models** ...

Example :

```
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcMSFootball.Models
{
    /// <summary>
    /// Represents a Player, belongs to a Team
    /// </summary>
    public class Player
    {
        [Key]
        public int ID { get; set; }        

        [Required(ErrorMessage = "You must enter the name")]
        [DisplayName("Player Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter the age")]
        [DisplayName("Player Age")]
        public int Age { get; set; }

        [Range(1, 10, ErrorMessage = "You must enter a valid player rating (1-10)")]
        [RegularExpression("^[0-9]{1,2}$", ErrorMessage = "Invalid Number")]
        [DisplayName("Player Rating (1-10)")]
        public int Rating { get; set; }

        [DisplayName("Comments")]
        public string Comments { get; set; }

        [DisplayName("Goals made")]
        [RegularExpression("^[0-9]{1,9}$", ErrorMessage = "Invalid Number")]
        public int Goals { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }

}
```

...where i learned about validations using annotations provided by ASP.NET. 

The implemented classes ( Player, Match and Team ) defined the **data model** used by the application. This approach is known as **code first**, and is used by **Entity Framework**. It allows the developer just worry about the classes and their relationships, while **EF** handles the DB creation task.

The relationship between the model classes are:

* A player has one team
* A team has a list of players
* A match has two teams

Using the **virtual** keyword means that **EF** should instantiate those objects.

After creating the Models i created the DBContext. 

using System.Data.Entity;

```
namespace MvcMSFootball.Models
{
    /// <summary>
    /// DBContext for the application
    /// </summary>
    public class FootballManagerDBContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FootballManagerDBContext, Config>());
        }
    }
}
```

This is how you tell the **EF** how to communicate with the DB.

At this point we have the model and the database initializer. We'll follow with the controllers. 
Controllers have actions which define functionality.

I created a controller for every model in the application ( Player, Team and Match ).

* The PlayerController allows the user to list players, filter them by name and team, detail them, create them and delete them.
* The TeamController allows the user to list teams, detail them, create them and delete them.
* The MatchController allows the user to list matches, create them and detail them.

```
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
```

Using controllers i learned about ViewBags, how to pass model data to their respective views, how to query Containers (such as List) using LINQ, how to work with dbContext to query the database and about ActionSelectors, like httpPost.

The views are generated automatically according to the model. Each view represents an action (create, edit, list, delete).

Here is the createPlayer view :

```
@model MvcMSFootball.Models.Player

@{
    ViewBag.Title = "Create";
}

<h2>Create</h2>

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

@using (Html.BeginForm()) {
    @Html.ValidationSummary(true)
    <fieldset>
        <legend>Player</legend>

        <div class="editor-label">
            @Html.LabelFor(model => model.Name)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.Name)
            @Html.ValidationMessageFor(model => model.Name)
        </div>

        <div class="editor-label">
            @Html.LabelFor(model => model.Age)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.Age)
            @Html.ValidationMessageFor(model => model.Age)
        </div>
        <div class="editor-label">
           Team name
        </div>
        <div class="editor-field">
         <!------------ Dropdown team selection --------- --->
            @Html.DropDownList("teamList", "-teams-")
            @Html.ValidationMessageFor(model => model.Team.ID)
        </div>
        <div class="editor-label">
            @Html.LabelFor(model => model.Comments)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.Comments)
            @Html.ValidationMessageFor(model => model.Comments)
        </div>
        <div class="editor-label">
            @Html.LabelFor(model => model.Rating)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.Rating)
            @Html.ValidationMessageFor(model => model.Rating)
        </div>
        <div class="editor-label">
            @Html.LabelFor(model => model.Goals)
        </div>
        <div class="editor-field">
            @Html.EditorFor(model => model.Goals)
            @Html.ValidationMessageFor(model => model.Goals)
        </div>

        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
}

<div>
    @Html.ActionLink("Back to List", "IndexList")
</div>
```

Working with views i learned about Razor Engine, the templates it generates, and how to develop html components using razor helpers.

As you can see in the createPlayer view i used a dropDownSelection populated with the teams created before. That dropDownSelector is populated in the PlayerController method **GenerateTeamSelect**.

```
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

```

This method populates the dropDownSelector with all the teams created for the create player action, and does the same for the edit player action but putting the player's team first ( so you don't have to select it again if you want to edit some other player's data like age or rating ).

This method is called before returning the views in the controllers actions :

```
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
```

The team details view was modified to show not only team information, but also to display the players included in that team. 

As for javascript i used the jQuery datePicker to select dates in the matches creation view. 

Minor modifications were made to the application **css**. I changed the background color to green and added a corresponding logo in the header.

**IMPORTANT**
=============

Had to change the db provider and the way i used to initialize the DB. This is because appHarbor won't let me create the DB.
In Web.config i used the following connectionString :
```
<add name="FootballManagerDBContext" connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=FM;Integrated Security=True;" providerName="System.Data.SqlClient" />
```
I removed the FootballManagerInitializer.cs file, because it's no longer useful.

I created a Configuration File 
```
namespace MvcMSFootball.Configuration
{
    public class Config : DbMigrationsConfiguration<FootballManagerDBContext>
    {
        public Config()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
```

And the DB Initializer is now included in the FootballManagerDBContext file.