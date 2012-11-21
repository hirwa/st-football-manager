using System.Data.Entity;

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
    }
}