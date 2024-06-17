using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Villa_API.Models
{
   public class Villa
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }
      public string Name { get; set; }

      public string Detalle { get; set; }

      [Required]
      public double Tarifa { get; set; }

      public int Ocupantes { get; set; }

      public int MetrosCuadrados { get; set; }

      public string ImagenUrl { get; set; }

      public string Amenidad { get; set; }

      public DateTime Created_at { get; set; }
      public DateTime Updated_at { get; set; }
   }
}
