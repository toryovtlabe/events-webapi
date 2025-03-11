using DataAccess.Repository.Interface;

namespace DataAccess.Repository.Service
{
    public abstract class Repository<T>(ApplicationContext context) : IRepository<T> where T : class
    {
        private readonly ApplicationContext _appContext = context;

        public void Create(T item)
        {
            _appContext.Add(item);
        }

        public void Delete(T? item)
        {
            try
            {
                _appContext.Remove(item);
            }
            catch { }
        }
        public void Update(T item)
        {
            _appContext.Update(item);
        }

        public List<T> GetAll()
        {
            return [.. _appContext.Set<T>()];
        }

        public T? GetById(int id)
        {
            return _appContext.Set<T>().Find(id);
        }

        public void Save()
        {
            _appContext.SaveChanges();
        }
    }
}
