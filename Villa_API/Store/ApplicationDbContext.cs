using Microsoft.EntityFrameworkCore;
using Villa_API.Models;

namespace Villa_API.Store
{
   public class ApplicationDbContext : DbContext
   {
      // inyeccion de dependencia en el constructor
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
      { }
      // indicamos que se cree una tabla en base de datos siguiendo el Modelo de la clase Villa
      public DbSet<Villa> Villas { get; set; }
      public DbSet<NumeroVilla> NumeroVillas { get; set; }

      // Un seeder para cargar datos en lla tabla villas
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Villa>().HasData(
            new Villa()
            {
               Id = 11,
               Name = "Villa Maravilla",
               Detalle = "Una hermosa villa con vista al mar",
               Tarifa = 300.50,
               Ocupantes = 4,
               MetrosCuadrados = 150,
               ImagenUrl = "http://ejemplo.com/imagen.jpg",
               Amenidad = "Piscina, Wi-Fi, Aire acondicionado",
               Created_at = DateTime.Now,
               Updated_at = DateTime.Now
            },
             new Villa()
             {
                Id = 12,
                Name = "Villa Sol y Playa",
                Detalle = "Un retiro tranquilo con acceso privado a la playa",
                ImagenUrl = "http://ejemplo.com/villa-solyplaya.jpg",
                Tarifa = 450.75,
                Ocupantes = 6,
                MetrosCuadrados = 200,
                Amenidad = "Jacuzzi, Sauna, Gimnasio, Wi-Fi",
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
             }

            );
      }
   }
}
