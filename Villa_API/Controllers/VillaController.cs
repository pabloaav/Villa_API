using Microsoft.AspNetCore.Mvc;
using Villa_API.Dto;
using Villa_API.Store;

namespace Villa_API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class VillaController : ControllerBase
   {

      /* GET ALL */
      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<IEnumerable<VillaDto>> GetVillas()
      {
         return Ok(VillaStore.villaList);
      }

      /* GET ONE */
      [HttpGet("{id}", Name = "GetVilla")]
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

      /* CREATE */
      [HttpPost]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status500InternalServerError)]
      public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto)
      {
         // VAlidacion Model State
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         // validacion personalizada
         if (VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
         {
            ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe.");
            return BadRequest(ModelState);
         }


         // Validación de entrada básica
         if (villaDto == null)
         {
            return BadRequest("El objeto de villa no puede ser nulo.");
         }

         if (villaDto.Id > 0)
         {
            return BadRequest("El ID de una nueva villa debe ser 0.");
         }

         var nextVillaId = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

         villaDto.Id = nextVillaId;

         // Almacenamiento de la nueva villa
         VillaStore.villaList.Add(villaDto);

         // Retorno de la villa creada con código de estado 201 Created
         return CreatedAtRoute("GetVilla", new { id = nextVillaId }, villaDto);
      }

      [HttpDelete("{id:int}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public IActionResult Delete(int id)
      {
         if (id == 0)
         {
            return BadRequest();
         }

         var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

         if (villa == null)
         {
            return NotFound();
         }

         VillaStore.villaList.Remove(villa);

         return NoContent();
      }

      [HttpPut("{id:int}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public IActionResult UpdateVilla(int id, [FromBody] VillaDto updatedVillaDto)
      {
         if (updatedVillaDto == null || id != updatedVillaDto.Id)
         {
            return BadRequest("El objeto de villa no puede ser nulo.");
         }

         if (id != updatedVillaDto.Id)
         {
            return BadRequest("El ID proporcionado no coincide con el ID de la villa.");
         }

         var existingVilla = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

         if (existingVilla == null)
         {
            return NotFound();
         }

         // Actualizar propiedades de la villa existente con los valores del objeto actualizado
         existingVilla.Nombre = updatedVillaDto.Nombre;
         existingVilla.MetrosCuadrados = updatedVillaDto.MetrosCuadrados;
         existingVilla.Ocupantes = updatedVillaDto.Ocupantes;
         // ... otras propiedades que necesitas actualizar

         // Retorno con código de estado 204 No Content
         return NoContent();
      }


   } // class
} // namespace
