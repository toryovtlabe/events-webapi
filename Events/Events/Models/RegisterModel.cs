namespace Events.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDay { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
