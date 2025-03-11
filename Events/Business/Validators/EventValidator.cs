using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.DTO;
using FluentValidation;

namespace Business.Validators
{
    public class EventValidator : AbstractValidator<EventDTO>
    {
        public EventValidator()
        {
            RuleFor(e => e.Name).Matches("^[а-яА-Яa-zA-Z0-9 _-]{3,50}$").WithMessage("Название может включать в себя русские и латинские символы, цифры и символы( ,_,-), длина от 3 до 50 символов");
            RuleFor(e => e.Description).Matches("^[а-яА-Яa-zA-Z0-9 _-]{3,999}$").WithMessage("Описание может включать в себя русские и латинские символы, цифры и символы( ,_,-), длина от 3 до 999 символов");
            RuleFor(e => e.Date).Must(BeAValidDate).WithMessage("Дата проведения не может быть в прошлом");
            RuleFor(e => e.MaxParticipants).InclusiveBetween(1,99999).WithMessage("Количество участников события ограничено от 1 до 99999");

        }
        private bool BeAValidDate(DateTime date)
        {
            return DateTime.Today <= date.Date;
        }
    }
}
