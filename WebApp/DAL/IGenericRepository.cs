using System.Collections.Generic;

namespace WebApp.DAL
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> Get();
        T GetByID(object id);
        void Add(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
    }
}
