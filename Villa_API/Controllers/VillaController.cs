using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Villa_API.Dto;
using Villa_API.Models;
using Villa_API.Store;

namespace Villa_API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class VillaController : ControllerBase
   {
      // Para acceder a base de datos se inyecta en dbContext en el constructor
      private readonly ILogger<VillaController> _logger;
      private readonly ApplicationDbContext _db;
      // Constructor
      public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
      {
         _logger = logger;
         _db = db;
      }

      /* GET ALL */
      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<IEnumerable<VillaDto>> GetVillas()
      {
         _logger.LogInformation("Obteniendo todas las villas");
         //return Ok(VillaStore.villaList);
         return Ok(_db.Villas.ToList());
      }

      /* GET ONE */
      [HttpGet("{id}", Name = "GetVilla")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]

      public ActionResult<VillaDto> GetVilla(int id)
      {
         if (id == 0)
         {
            _logger.LogError("Error al obtener la villa con id {0}", id);
            return BadRequest();
         }
         //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
         var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
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
         if (_db.Villas.FirstOrDefault(v => v.Name.ToLower() == villaDto.Nombre.ToLower()) != null)
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

         //var nextVillaId = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

         //villaDto.Id = nextVillaId;

         // Almacenamiento de la nueva villa
         //VillaStore.villaList.Add(villaDto);

         // copiar datos del DTO al Modelo
         Villa tempModel = new Villa()
         {

            Name = villaDto.Nombre,
            Detalle = villaDto.Detalle,
            ImagenUrl = villaDto.ImagenUrl,
            Ocupantes = villaDto.Ocupantes,
            Tarifa = villaDto.Tarifa,
            MetrosCuadrados = villaDto.MetrosCuadrados,
            Amenidad = villaDto.Amenidad
         };
         _db.Villas.Add(tempModel);
         _db.SaveChanges();

         // Retorno de la villa creada con código de estado 201 Created
         return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
      }

      /* DELETE */
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

         var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

         if (villa == null)
         {
            return NotFound();
         }

         _db.Villas.Remove(villa);
         _db.SaveChanges();

         return NoContent();
      }

      /* PUT */
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

         // copiar datos del DTO al Modelo
         Villa tempModel = new Villa()
         {
            Id = updatedVillaDto.Id,
            Name = updatedVillaDto.Nombre,
            Detalle = updatedVillaDto.Detalle,
            ImagenUrl = updatedVillaDto.ImagenUrl,
            Ocupantes = updatedVillaDto.Ocupantes,
            Tarifa = updatedVillaDto.Tarifa,
            MetrosCuadrados = updatedVillaDto.MetrosCuadrados,
            Amenidad = updatedVillaDto.Amenidad
         };
         _db.Villas.Update(tempModel);
         _db.SaveChanges();

         // Retorno con código de estado 204 No Content
         return NoContent();
      }

      /* PATCH */
      [HttpPatch("{id:int}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchVillaDto)
      {
         if (patchVillaDto == null || id == 0)
         {
            return BadRequest("El objeto de villa no puede ser nulo ni el id 0");
         }

         // se busca el registro a modificar
         var existingVilla = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

         if (existingVilla == null)
         {
            return NotFound();
         }

         // se utiliza el paquete para aplicar los cambios. Se le pasa como segundo parametro model state para validar
         //patchVillaDto.ApplyTo(existingVilla, ModelState);

         // copiar los valores de una villa existente al DTO temporal
         VillaDto villaDto = new VillaDto()
         {
            Id = existingVilla.Id,
            Nombre = existingVilla.Name,
            Detalle = existingVilla.Detalle,
            ImagenUrl = existingVilla.ImagenUrl,
            Ocupantes = existingVilla.Ocupantes,
            Tarifa = existingVilla.Tarifa,
            MetrosCuadrados = existingVilla.MetrosCuadrados,
            Amenidad = existingVilla.Amenidad
         };

         // aplicar el objeto patch a el DTO temporal
         patchVillaDto.ApplyTo(villaDto, ModelState);

         // se pregunta por el model state, para que los cambios sean consistentes
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         // ahora villaDto contiene los cambios aplicados. Pasamos el DTO al modelo temporal para guardar los cambios
         Villa modelo = new Villa()
         {
            Id = villaDto.Id,
            Name = villaDto.Nombre,
            Detalle = villaDto.Detalle,
            ImagenUrl = villaDto.ImagenUrl,
            Ocupantes = villaDto.Ocupantes,
            Tarifa = villaDto.Tarifa,
            MetrosCuadrados = villaDto.MetrosCuadrados,
            Amenidad = villaDto.Amenidad
         };

         _db.Villas.Update(modelo);
         _db.SaveChanges();


         // Retorno con código de estado 204 No Content
         return NoContent();
      }

   } // class
} // namespace
