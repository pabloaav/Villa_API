using System.ComponentModel.DataAnnotations;

namespace Villa_API.Dto
{
   public class VillaDto
   {
      public int Id { get; set; }
      [MaxLength(30)]
      [Required]
      public string Nombre { get; set; }
      public DateTime FechaCreacion => DateTime.Now;

      public int Ocupantes { get; set; }
      public int MetrosCuadrados { get; set; }
   }
}
