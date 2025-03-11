namespace DataAccess.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDay { get; set; }
        public string Email { get; set; }
        public string Login {  get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string RefreshToken { get; set; }
        public List<Subscription> Subscriptions { get; set; } = new();
    }
}
