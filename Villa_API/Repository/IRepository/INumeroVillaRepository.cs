using Villa_API.Models;

namespace Villa_API.Repository.IRepository
{
   // Porposito: contrato para actualizar una modelo Villa
   // Hereda de la Interface repositorio general
   public interface INumeroVillaRepository : IRepository<NumeroVilla>
   {
      Task<NumeroVilla> Update(NumeroVilla entity);
   }
}
