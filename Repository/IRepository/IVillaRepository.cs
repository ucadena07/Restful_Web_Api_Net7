using MagicVillaApi.Models;
using System.Linq.Expressions;

namespace MagicVillaApi.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task Create(Villa entity);
        Task Remove(Villa entity);
        Task Save();
        Task<List<Villa>> GetAll(Expression<Func<Villa>> filter = null);
        Task<List<Villa>> Get(Expression<Func<Villa>> filter = null, bool tracked = true);
    }
}
