using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Weapon
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CommonRange { get; set; }
        public long MaxRange { get; set; }
        public decimal Caliber { get; set; }
        public string Class { get; set; }
        public string Manufacturer { get; set; }
    }
}
