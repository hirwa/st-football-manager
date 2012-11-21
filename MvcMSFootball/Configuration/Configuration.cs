using System.Data.Entity.Migrations;
using MvcMSFootball.Models;

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