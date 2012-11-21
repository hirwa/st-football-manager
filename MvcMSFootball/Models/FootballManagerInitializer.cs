using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MvcMSFootball.Configuration;

namespace MvcMSFootball.Models
{
    /// <summary>
    /// Database Initializer class
    /// </summary>
    public class FootballManagerInitializer : MigrateDatabaseToLatestVersion<FootballManagerDBContext, Config>
    {
        /// <summary>
        /// Seeds the database with data
        /// </summary>
        protected override void Seed(FootballManagerDBContext context)
        {
            var teams = new List<Team> {  
  
                 new Team { Name = "MakingSense Team A",   
                             Won = 2,   
                             Draw = 1,  
                             Lost = 2  
                             },  

                 new Team { Name = "MakingSense Team B",   
                          Won = 2,   
                          Draw = 1,  
                          Lost = 1  
                          },   
             };

            teams.ForEach(d => context.Teams.Add(d));
            context.SaveChanges();

            var players = new List<Player> {  
  
                new Player{
                Name = "Cosme Fulanito",
                Age = 27,
                Comments = "He has magic in his feet",
                Rating = 8,
                TeamId = 1,
                Team = context.Teams.Single( s => s.ID == 1),
                Goals = 2
                },  

                new Player{
                Name = "Matias Farulla",
                Age = 25,
                Comments = "Rough player",
                Rating = 7,
                TeamId = 2,   
                Team = context.Teams.Single( s => s.ID == 2),
                Goals = 0
                },

                new Player{
                Name = "Eustaquio Gomez",
                Age = 33,
                Comments = "Close to retire",
                Rating = 5,
                TeamId = 1,
                Team = context.Teams.Single( s => s.ID == 1),
                Goals = 1
                },  

                new Player{
                Name = "Siloquio Hernandez",
                Age = 19,
                Comments = "Very Fast Winger",
                Rating = 8,
                TeamId = 2,   
                Team = context.Teams.Single( s => s.ID == 2),
                Goals = 3
                },

                new Player{
                Name = "Herminio Bunga",
                Age = 30,
                Comments = "Experienced guy",
                Rating = 7,
                TeamId = 1,
                Team = context.Teams.Single( s => s.ID == 1),
                Goals = 4
                },  

                new Player{
                Name = "Herman Jones",
                Age = 28,
                Comments = "Foreign player",
                Rating = 4,
                TeamId = 2,   
                Team = context.Teams.Single( s => s.ID == 2),
                Goals = 0
                }  
    

             };

            players.ForEach(d => context.Players.Add(d));

            context.SaveChanges();

            var matches = new List<Match> {  
  
                 new Match { HomeTeamId = 1,
                             AwayTeamId = 2,
                             HomeGoals = 6,   
                             AwayGoals = 4,  
                             Date = DateTime.Parse("03/10/2012")  
                             },  

                 new Match { HomeTeamId = 2,
                             AwayTeamId = 1,
                             HomeGoals = 5,   
                             AwayGoals = 5,  
                             Date = DateTime.Parse("07/10/2012")  
                             }  
             };

            matches.ForEach(d => context.Matches.Add(d));
            context.SaveChanges();
        }
    }
}