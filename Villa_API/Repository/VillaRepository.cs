using Villa_API.Models;
using Villa_API.Repository.IRepository;
using Villa_API.Store;

namespace Villa_API.Repository
{
   // Implementa interfaz de Villa Repository
   public class VillaRepository : Repository<Villa>, IVillaRepository
   {
      private readonly ApplicationDbContext _db;
      public VillaRepository(ApplicationDbContext db) : base(db)
      {
         _db = db;
      }

      public async Task<Villa> Update(Villa entity)
      {
         entity.Created_at = DateTime.Now;
         _db.Villas.Update(entity);
         await _db.SaveChangesAsync();
         return entity;
      }
   }

}
