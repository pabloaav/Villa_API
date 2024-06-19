using System.ComponentModel.DataAnnotations;

namespace Villa_API.Dto
{
   public class VillaCreateDto
   {
      public int Id { get; set; }
      [MaxLength(30)]
      [Required]
      public string Nombre { get; set; }
      public DateTime FechaCreacion => DateTime.Now;
      public int Ocupantes { get; set; }
      public int MetrosCuadrados { get; set; }
      public string Detalle { get; set; }
      [Required]
      public double Tarifa { get; set; }
      public string ImagenUrl { get; set; }
      public string Amenidad { get; set; }
   }
}
