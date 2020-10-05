using Skybook.Data;
using Skybook.Models.Planet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybook.Models
{
    public class StarSystemDetail
    {
        public int StarSystemId { get; set; }
        [Display(Name="Name of Star System")]
        public string Name { get; set; }
        [Display(Name="Dominate Lifeforms")]
        public string Race { get; set; }
        [Display(Name="Type of Economy")]
        public string Economy { get; set; }
        [Display(Name="State of Conflic")]
        public string Conflict { get; set; }

        // Below is for the planets to be listed with the star system
        public List<PlanetListItem> Planets { get; set; }
        public int PlanetId { get; set; }
        [Display(Name = "Name of Planet")]
        public string PlanetName { get; set; }
        [Display(Name = "The Planets Type")]
        public string PlanetType { get; set; }
        [Display(Name = "Most Common Minerals")]
        public string Minerals { get; set; }
        [Display(Name = "Special Items buried here")]
        public string SpecialBuried { get; set; }
        public string SentinelActivity { get; set; }

        
    }
}
