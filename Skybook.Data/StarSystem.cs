using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybook.Data
{
   public class StarSystem
    {
        [Key]
        public int StarSystemId { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Economy { get; set; }
        public string Conflict { get; set; }
        public Guid OwnerId { get; set; }
        

    }
}
