namespace DataAccess.Entity
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Place Place {  get; set; }
        public List<Category> Categories { get; set; }
        public int MaxParticipants { get; set; }
        public List<Subscription> Subscriptions { get; set; } = new();
        public string Image { get; set; }
    }
}
