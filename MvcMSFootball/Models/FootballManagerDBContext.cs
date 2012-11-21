using System.Data.Entity;
using MvcMSFootball.Configuration;

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