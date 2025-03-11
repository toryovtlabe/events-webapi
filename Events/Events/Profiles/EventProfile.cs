using AutoMapper;
using Business.DTO;
using DataAccess.Entity;
using Events.Models;

namespace Events.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventModel, EventDTO>();
        }
    }
}
