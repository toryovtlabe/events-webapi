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
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => 0));
            CreateMap<LoginDTO, User>();
            CreateMap<User, UserDTO>();
        }
    }
}
