using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.DTO;
using FluentValidation;

namespace Business.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(u => u.Login).Matches("^[a-zA-Z0-9_-]{5,30}$").WithMessage("Логин может включать в себя латинские символы, цифры и символы(_,-), длина от 5 до 30 символов");
            RuleFor(u => u.Password).Matches("^[a-zA-Z0-9_-]{5,30}$").WithMessage("Пароль может включать в себя латинские символы, цифры и символы(_,-), длина от 5 до 30 символов");
            RuleFor(u => u.FirstName).Matches("^[a-zA-Zа-яА-ЯёЁ]{3,30}$").WithMessage("Длина имени должна быть больше 3 и не больше 30 символов.");
            RuleFor(u => u.Surname).Matches("^[a-zA-Zа-яА-ЯёЁ]{3,40}$").WithMessage("Длина фамилии должна быть больше 3 и не больше 40 символов.");
            RuleFor(u => u.Email).Matches("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$").Length(6,255).WithMessage("Неверный формат Email.");
            RuleFor(u => u.BirthDay).Must(BeAValidAge).WithMessage("Возраст должен быть больше 0 и меньше 120 лет.");
        }

        private bool BeAValidAge(DateOnly date)
        {
            int age = DateTime.Today.Year - date.Year;
            return age > 0 && age < 100;
        }
    }
}
