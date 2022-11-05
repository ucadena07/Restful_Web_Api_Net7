using MagicVillaApi.Models;
using MagicVillaApi.Respositories.IRepositories;
using System.Linq.Expressions;

namespace MagicVillaApi.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> Update(Villa entity);
    }
}
