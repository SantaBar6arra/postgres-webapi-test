using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Soldier
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public long PrimaryWeaponId { get; set; }
        public long SecondaryWeaponId { get; set; }
    }
}
