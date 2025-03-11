using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class RegisterDTO
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDay { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
