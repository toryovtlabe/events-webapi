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
    public class SubProfile : Profile
    {
        public SubProfile()
        {
            CreateMap<Subscription, SubDTO>();
        }
    }
}
