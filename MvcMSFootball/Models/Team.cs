using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcMSFootball.Models
{
    /// <summary>
    /// Represents a Team
    /// </summary>
    public class Team
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "You must enter the name")]
        [DisplayName("Team name")]
        public string Name { get; set; }

        public int Won { get; set; }
        
        public int Draw { get; set; }
        
        public int Lost { get; set; }

        public virtual List<Player> Players { get; set; }
    }

}