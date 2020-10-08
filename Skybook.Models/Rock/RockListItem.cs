using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybook.Models.Rock
{
    public class RockListItem
    {
        public int RockId { get; set; }
        public string Name { get; set; }
        public string PrimaryElement { get; set; }
        public string SecondaryElement { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }


        [ForeignKey(nameof(Planet))]
        public int PlanetId { get; set; }
        public virtual Skybook.Data.Planet Planet { get; set; }
    }
}
