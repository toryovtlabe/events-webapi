using DataAccess.Entity;

namespace Events.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public List<string> Categories { get; set; }
        public int MaxParticipants { get; set; }
        public string Image { get; set; }
    }
}
