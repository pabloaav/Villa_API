using System.Net;

namespace Villa_API.Models
{
   public class ApiResponse
   {

      public HttpStatusCode StatusCode { get; set; }
      public bool IsExitoso { get; set; } = true;
      public List<string> ErrorMessages { get; set; }
      public object Resultado { get; set; }

   }
}
