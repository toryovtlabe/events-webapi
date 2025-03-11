using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entity;
using DataAccess.Repository.Service;

namespace DataAccess.Repository.Services
{
    public class CategoryRepository(ApplicationContext context) : Repository<Category>(context)
    {
        private readonly ApplicationContext _appContext = context;
        public Category? GetByName(string name)
        {
            return _appContext.Set<Category>().FirstOrDefault(c => c.Name == name);
        }
    }
}
