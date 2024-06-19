using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villa_API.Store;

namespace Villa_API.Repository.IRepository
{
   // es la clase que implementa el IRepository
   public class Repository<T> : IRepository<T> where T : class
   {
      private readonly ApplicationDbContext _db;
      internal DbSet<T> dbSet; // no hace falta el guion bajo
      public Repository(ApplicationDbContext db)
      {
         _db = db;
         dbSet = _db.Set<T>();
      }
      public async Task Create(T entity)
      {
         await dbSet.AddAsync(entity);
         await Save();
      }

      public async Task Save()
      {
         await _db.SaveChangesAsync();
      }

      public async Task<T> Get(Expression<Func<T, bool>> filter, bool tracked = true)
      {
         IQueryable<T> query = dbSet;

         query = tracked ? query : query.AsNoTracking();

         if (filter != null)
         {
            query = query.Where(filter);
         }

         return await query.FirstOrDefaultAsync();
      }

      public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
      {
         IQueryable<T> query = dbSet;

         if (filter != null)
         {
            query = query.Where(filter);
         }

         return await query.ToListAsync();
      }

      public Task Remove(T entity)
      {
         dbSet.Remove(entity);
         return Save(); // aqui adentro se graban los cambios en la base de datos
      }


   }
}
