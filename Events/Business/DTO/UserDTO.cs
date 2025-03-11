using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entity;

namespace Business.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDay { get; set; }
        public string Email { get; set; }
        public List<Subscription> Subscriptions { get; set; } = new();
    }
}
