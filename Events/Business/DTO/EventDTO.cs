using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entity;

namespace Business.DTO
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public List<string> Categories { get; set; }
        public int MaxParticipants { get; set; }
        public int FreeTickets { get; set; }
        public string Image { get; set; }
    }
}
