using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Villa_API.Models
{

   public class NumeroVilla
   {
      [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
      public int VillaNo { get; set; }

      [Required]
      public int VillaId { get; set; }

      [ForeignKey("VillaId")]
      public Villa Villa { get; set; }

      public string DetalleEspecial { get; set; }

      public DateTime Created_at { get; set; } = DateTime.Now;
      public DateTime Updated_at { get; set; } = DateTime.Now;
   }

}
