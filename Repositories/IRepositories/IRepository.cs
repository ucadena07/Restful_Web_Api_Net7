using MagicVillaApi.Models;
using System.Linq.Expressions;

namespace MagicVillaApi.Respositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);
        Task Remove(T entity);
        Task Save();
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null);
        Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true);
    }
}
