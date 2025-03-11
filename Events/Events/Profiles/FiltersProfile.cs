using AutoMapper;
using Business.DTO;
using Events.Models;

namespace Events.Profiles
{
    public class FiltersProfile : Profile
    {
        public FiltersProfile()
        {
            CreateMap<FiltersEventModel, FiltersEventDTO>();
        }
    }
}
