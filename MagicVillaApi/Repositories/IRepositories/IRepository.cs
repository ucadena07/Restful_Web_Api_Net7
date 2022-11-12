using MagicVillaApi.Models;
using System.Linq.Expressions;

namespace MagicVillaApi.Respositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);
        Task Remove(T entity);
        Task Save();
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = "",
            int pageSize = 3, int pageNumber = 1);
        Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true, string includeProperties = "");
    }
}
