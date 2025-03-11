using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using DataAccess.Entity;

namespace Business.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventDTO, Event>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(name => new Category { Name = name }).ToList()))
                .ForMember(dest => dest.Place, opt => opt.MapFrom(src => new Place { Name = src.Place}));
            CreateMap<Event, EventDTO>()
                .ForMember(dest => dest.FreeTickets, opt => opt.MapFrom(src => src.MaxParticipants - src.Subscriptions.Count()))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)))
                .ForMember(dest => dest.Place, opt => opt.MapFrom(src => src.Place.Name));


        }
    }
}
