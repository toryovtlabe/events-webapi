using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Event> Event { get; set; }
    }
}
