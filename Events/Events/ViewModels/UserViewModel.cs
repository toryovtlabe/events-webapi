﻿namespace Events.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDay { get; set; }
        public string Email { get; set; }
    }
}
