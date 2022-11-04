using MagicVillaApi.Models;
using MagicVillaApi.Respositories.IRepositories;

namespace MagicVillaApi.Repositories.IRepositories
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> Update(VillaNumber entity);
    }
}
