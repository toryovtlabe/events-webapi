using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.DTO;
using FluentValidation;

namespace Business.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(u => u.Login).Matches("^[a-zA-Z0-9_-]{5,30}$").WithMessage("Логин может включать в себя латинские символы, цифры и символы(_,-), длина от 5 до 30 символов");
            RuleFor(u => u.Password).Matches("^[a-zA-Z0-9_-]{5,30}$").WithMessage("Пароль может включать в себя латинские символы, цифры и символы(_,-), длина от 5 до 30 символов");
        }
    }
}
