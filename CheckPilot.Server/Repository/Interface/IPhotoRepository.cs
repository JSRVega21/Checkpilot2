using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CheckPilot.Server.Repository

{
    public interface IPhotoRepository<T, TKey>
    {
        IList<T> GetList();
        T GetByKey(TKey key);
        T GetByKey(TKey key, bool tracking = false);
        T Add(T entity);
        T Update(T entity);
        void Delete(TKey key);

        Task<IList<T>> GetListAsync();
        Task<T> GetByKeyAsync(TKey key);
        Task<T> GetByKeyAsync(TKey key, bool tracking = false);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(TKey key);

        Task<IList<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}


