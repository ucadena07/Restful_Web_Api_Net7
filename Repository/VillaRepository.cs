using AutoMapper;
using MagicVillaApi.Data;
using MagicVillaApi.Models;
using MagicVillaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVillaApi.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task Create(Villa entity)
        {
            await _db.Villas.AddAsync(entity);
            await Save();
        }

        public async Task<Villa> Get(Expression<Func<Villa,bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _db.Villas;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
                query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter = null)
        {
            IQueryable<Villa> query = _db.Villas;
            if(filter != null)
                query = query.Where(filter);
            return await query.ToListAsync();

        }

        public async Task Remove(Villa entity)
        {
            _db.Remove(entity); 
            await Save();   
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task Update(Villa entity)
        {
            _db.Villas.Update(entity);
            await Save();
        }
    }
}
