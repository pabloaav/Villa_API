using System.Linq.Expressions;

namespace Villa_API.Repository.IRepository
{
   // Un IRepository es una interface generica, que recibe cualquier tipo de entidad
   public interface IRepository<T> where T : class
   {
      Task Create(T entity);
      // si no se envia un filtro, devuelve toda la lista. Sino, filtra segun la funcion
      Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
      Task<T> Get(Expression<Func<T, bool>> filter, bool tracked = true);
      Task Remove(T entity);
      // se encarga de SaveChanges de DbContext en la base de datos
      Task Save();
   }


}
