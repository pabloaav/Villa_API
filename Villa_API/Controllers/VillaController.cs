using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Villa_API.Dto;
using Villa_API.Models;
using Villa_API.Store;

namespace Villa_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        public  IEnumerable<VillaDto> GetVillas()
        {
            return VillaStore.villaList;
        }
    }
}
