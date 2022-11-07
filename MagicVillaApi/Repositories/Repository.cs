using AutoMapper;
using MagicVillaApi.Data;
using MagicVillaApi.Models;
using MagicVillaApi.Respositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVillaApi.Respositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        internal DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _dbSet = _db.Set<T>();  
        }
        public async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);
            await Save();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
                query = query.Where(filter);

            if(includeProperties!= null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);
			if (includeProperties != null)
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}


			return await query.ToListAsync();
        }

        public async Task Remove(T entity)
        {
            _dbSet.Remove(entity);
            await Save();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
