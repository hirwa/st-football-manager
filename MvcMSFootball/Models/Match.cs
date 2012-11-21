using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcMSFootball.Models
{
    /// <summary>
    /// Represents a Match played by two teams
    /// </summary>
    public class Match
    {
        [Key]
        public int ID { get; set; }

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Required field")]
        [DisplayName("Home Team Goals made")]
        [RegularExpression("^[0-9]{1,9}$", ErrorMessage = "Invalid Number")]
        public int HomeGoals { get; set; }

        [Required(ErrorMessage = "Required field")]
        [DisplayName("Away Team Goals made")]
        [RegularExpression("^[0-9]{1,9}$", ErrorMessage = "Invalid Number")]
        public int AwayGoals { get; set; }
    }
}