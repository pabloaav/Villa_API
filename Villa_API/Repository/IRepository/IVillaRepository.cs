using Villa_API.Models;

namespace Villa_API.Repository.IRepository
{
   // Porposito: contrato para actualizar una modelo Villa
   // Hereda de la Interface repositorio general
   public interface IVillaRepository : IRepository<Villa>
   {
      Task<Villa> Update(Villa entity);
   }
}
