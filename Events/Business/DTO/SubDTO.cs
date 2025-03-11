using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entity;

namespace Business.DTO
{
    public class SubDTO
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }

    }
}
