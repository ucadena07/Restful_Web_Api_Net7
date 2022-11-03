using MagicVillaApi.Models;
using System.Linq.Expressions;

namespace MagicVillaApi.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task Create(Villa entity);
        Task Update(Villa entity);
        Task Remove(Villa entity);
        Task Save();
        Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter = null);
        Task<Villa> Get(Expression<Func<Villa,bool>> filter = null, bool tracked = true);
    }
}
