using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybook.Models.Animal
{
   public class AnimalDetail
    {
        
        public int AnimalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        [ForeignKey(nameof(Planet))]
        public int PlanetId { get; set; }
        public virtual Skybook.Data.Planet Planet { get; set; }
    }
}
