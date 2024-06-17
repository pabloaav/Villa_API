using Microsoft.EntityFrameworkCore;
using Villa_API.Models;

namespace Villa_API.Store
{
   public class ApplicationDbContext : DbContext
   {
      // inyeccion de dependencia en el constructor
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
      {

      }
      // indicamos que se cree una tabla en base de datos siguiendo el Modelo de la clase Villa
      DbSet<Villa> Villas { get; set; }
   }
}
