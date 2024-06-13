using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Villa_API.Models;

namespace Villa_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        public  IEnumerable<Villa> GetVillas()
        {
            return new List<Villa>
            {
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa"
                },
                new Villa {
                    Id = 2,
                    Name = "Premium Villa"
                }
            };
        }
    }
}
