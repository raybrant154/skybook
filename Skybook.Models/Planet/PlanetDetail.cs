using Skybook.Data;
using Skybook.Models.Animal;
using Skybook.Models.Plant;
using Skybook.Models.Rock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybook.Models.Planet
{
    public class PlanetDetail
    {
        public int PlanetId { get; set; }
        [Display(Name="Name of Planet")]
        public string Name { get; set; }
        [Display(Name="The Planets Type")]
        public string PlanetType { get; set; }
        [Display(Name="Most Common Minerals")]
        public string Minerals { get; set; }
        [Display(Name="Special Items here")]
        public string SpecialBuried { get; set; }
        public string SentinelActivity { get; set; }
        public byte[] Image { get; set; }


        [ForeignKey(nameof(StarSystem))]
        public int StarSystemId { get; set; }
        public virtual StarSystem StarSystem { get; set; }

        //Below is for Animal

        public List<AnimalListItem> Animals { get; set; }
        public int AnimalId { get; set; }
        public string AnimalName { get; set; }
        public string Description { get; set; }

        //Below is for Plant

        public List<PlantListItem> Plants { get; set; }
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public string PrimaryElement { get; set; }
        public string SecondaryElement { get; set; }
        public string PlantDescription { get; set; }

        //Below is for Rock

        public List<RockListItem> Rocks { get; set; }
        public int RockId { get; set; }
        public string RockName { get; set; }
        public string RockPrimaryElement { get; set; }
        public string RockSecondaryElement { get; set; }
        public string RockDescription { get; set; }
    }
}
