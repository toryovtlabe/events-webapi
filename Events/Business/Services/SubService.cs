using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using DataAccess.Entity;
using DataAccess.Repository.Service;
using FluentValidation;

namespace Business.Services
{
    public class SubService(SubscriptionRepository subRepository,EventRepository eventRepository, UserRepository userRepository, IMapper mapper)
    {
        private readonly EventRepository _eventRepository = eventRepository;
        private readonly UserRepository _userRepository = userRepository;
        private readonly SubscriptionRepository _subRepository = subRepository;

        private readonly IMapper _mapper = mapper;

        public SubDTO CreateSub(int eventId, string userLogin)
        {
            Subscription subscription = null;
            if (eventId > 0) {
                if (userLogin != null)
                {
                    Event? @event = _eventRepository.GetById(@eventId);
                    User? user = _userRepository.GetByLogin(userLogin);
                    if (@event != null && user != null)
                    {
                        if (user.Subscriptions.FirstOrDefault(s => s.Event.Id == eventId) == null)
                        {
                            subscription = new Subscription() { Event = @event, User = user };
                            _subRepository.Create(subscription);
                            _subRepository.Save();
                        }
                        else throw new InvalidOperationException("Пользователь уже подписан на это событие");
                    }
                    else throw new DirectoryNotFoundException("Событие или пользователь не найдены");
                }
                else throw new ArgumentException("Пользователь должен быть авторизирован");
            }
            else
            {
                throw new ValidationException("id события должен быть больше 0");
            }
            SubDTO sub = _mapper.Map<SubDTO>(subscription);
            return sub;
        }

        public List<UserDTO> GetSubsById(int eventId)
        {
            Event? @event = null;
            List<UserDTO> subs = [];
            if (eventId > 0)
            {
                @event = _eventRepository.GetById(eventId);
            }
            else
            {
                throw new ValidationException("id события должен быть больше 0");
            }
            if(@event != null)
            {
                foreach (Subscription sub in @event.Subscriptions)
                {
                    subs.Add(_mapper.Map<UserDTO>(sub.User));
                }
            }
            else
            {
                throw new DirectoryNotFoundException("Событие не найдено");
            }
            return subs;
        }

        public UserDTO GetUserById(int id)
        {
            User? user = _userRepository.GetById(id);
            if(user == null)
            {
                throw new DirectoryNotFoundException("Пользователь не найден.");
            }
            return _mapper.Map<UserDTO>(user);
        }

        public void DeleteSub(int eventId, string userLogin)
        {
            Subscription? subscription = null;
            if (eventId > 0)
            {
                if (userLogin != null)
                {
                    Event? @event = _eventRepository.GetById(@eventId);
                    User? user = _userRepository.GetByLogin(userLogin);
                    if (@event != null && user != null)
                    {
                        if ((subscription = user.Subscriptions.FirstOrDefault(s => s.Event.Id == eventId)) != null)
                        {
                            _subRepository.Delete(subscription);
                            _subRepository.Save();
                        }
                        else throw new InvalidOperationException("Пользователь не подписан на выбранное событие.");
                    }
                    else throw new DirectoryNotFoundException("Событие или пользователь не найдены");
                }
                else throw new ArgumentException("Пользователь должен быть авторизирован");
            }
            else
            {
                throw new ValidationException("id события должен быть больше 0");
            }
        }
    }
}
