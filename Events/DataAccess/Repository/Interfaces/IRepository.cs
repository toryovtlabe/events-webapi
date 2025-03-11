namespace DataAccess.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        List<T> GetAll();
        T? GetById(int id);
        void Save();
    }
}
