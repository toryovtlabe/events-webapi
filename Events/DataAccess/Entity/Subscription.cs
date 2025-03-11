namespace DataAccess.Entity
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Event Event { get; set; }
        public User User { get; set; }
    }
}
