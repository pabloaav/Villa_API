﻿using Microsoft.EntityFrameworkCore;
using Villa_API.Models;

namespace Villa_API.Store
{
   public class ApplicationDbContext : DbContext
   {
      // inyeccion de dependencia en el constructor
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
      { }
      // indicamos que se cree una tabla en base de datos siguiendo el Modelo de la clase Villa
      DbSet<Villa> Villas { get; set; }

      // Un seeder para cargar datos en lla tabla villas
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Villa>().HasData(
            new Villa()
            {
               Id = 1,
               Name = "Villa Maravilla",
               Detalle = "Una hermosa villa con vista al mar",
               Tarifa = 300.50,
               Ocupantes = 4,
               MetrosCuadrados = 150,
               ImagenUrl = "http://ejemplo.com/imagen.jpg",
               Amenidad = "Piscina, Wi-Fi, Aire acondicionado",
               Created_at = DateTime.Now,
               Updated_at = DateTime.Now
            }
            );
      }
   }
}
