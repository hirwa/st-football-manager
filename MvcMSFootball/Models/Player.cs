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