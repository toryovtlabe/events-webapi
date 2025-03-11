using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entity;
using DataAccess.Repository.Service;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository.Services
{
    public class PlaceRepository(ApplicationContext context) : Repository<Place>(context) 
    { 
        private readonly ApplicationContext _appContext = context;
        public Place? GetByName(string name)
        {
            return _appContext.Set<Place>().FirstOrDefault(p => p.Name == name);
        }
    }
}
