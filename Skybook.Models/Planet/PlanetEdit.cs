using Skybook.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybook.Models.Planet
{
    public class PlanetEdit
    {
        public int PlanetId { get; set; }
        public string Name { get; set; }
        public string PlanetType { get; set; }
        public string Minerals { get; set; }
        public string SpecialBuried { get; set; }
        public string SentinelActivity { get; set; }

        [ForeignKey(nameof(StarSystem))]
        public int StarSystemId { get; set; }
        public virtual StarSystem StarSystem { get; set; }
    }
}
