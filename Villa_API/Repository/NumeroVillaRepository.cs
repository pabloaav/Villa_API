using Villa_API.Models;
using Villa_API.Repository.IRepository;
using Villa_API.Store;

namespace Villa_API.Repository
{
   // Implementa interfaz de Villa Repository
   public class NumeroVillaRepository : Repository<NumeroVilla>, INumeroVillaRepository
   {
      private readonly ApplicationDbContext _db;
      public NumeroVillaRepository(ApplicationDbContext db) : base(db)
      {
         _db = db;
      }

      public async Task<NumeroVilla> Update(NumeroVilla entity)
      {
         entity.Updated_at = DateTime.Now;
         _db.NumeroVillas.Update(entity);
         await _db.SaveChangesAsync();
         return entity;
      }
   }

}
