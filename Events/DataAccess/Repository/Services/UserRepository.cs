using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entity;
using DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository.Service
{
    public class UserRepository(ApplicationContext context) : Repository<User>(context)
    {
        private readonly ApplicationContext _appContext = context;
        public User? GetByLogin(string login)
        {
            return _appContext.Set<User>().Include(u=>u.Subscriptions).ThenInclude(s=>s.Event).FirstOrDefault(u => u.Login == login);
        }

        public new User? GetById(int id)
        {
            return _appContext.Set<User>().Include(u => u.Subscriptions).FirstOrDefault(u => u.Id == id);
        }
    }
}
