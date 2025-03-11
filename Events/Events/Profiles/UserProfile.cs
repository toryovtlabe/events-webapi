using AutoMapper;
using Business.DTO;
using DataAccess.Entity;
using Events.Models;
using Events.ViewModels;

namespace Events.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterModel, RegisterDTO>();
            CreateMap<LoginModel, LoginDTO>();
            CreateMap<UserDTO, UserViewModel>();
        }
    }
}
