using Microsoft.AspNetCore.Mvc;
using Villa_API.Dto;
using Villa_API.Store;

namespace Villa_API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class VillaController : ControllerBase
   {
      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<IEnumerable<VillaDto>> GetVillas()
      {
         return Ok(VillaStore.villaList);
      }

      [HttpGet("{id}")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]

      public ActionResult<VillaDto> GetVilla(int id)
      {
         if (id == 0) { return BadRequest(); }
         var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
         if (villa == null)
         {
            return NotFound();
         }

         return Ok(villa);

      }

      [HttpPost]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status500InternalServerError)]
      public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto)
      {
         // Validación de entrada básica
         if (villaDto == null)
         {
            return BadRequest("El objeto de villa no puede ser nulo.");
         }

         if (villaDto.Id > 0)
         {
            return BadRequest("El ID de una nueva villa debe ser 0.");
         }

         var nextVilla = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

         // Almacenamiento de la nueva villa
         VillaStore.villaList.Add(villaDto);

         // Retorno de la villa creada con código de estado 201 Created
         return CreatedAtAction(nameof(GetVilla), new { id = nextVilla }, villaDto);
      }
   }
}
