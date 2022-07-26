using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ObjectId
    {
        public ObjectId(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }
}
