using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Villa_API.Dto;
using Villa_API.Models;
using Villa_API.Repository.IRepository;

namespace Villa_API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class VillaController : ControllerBase
   {
      // Para acceder a base de datos se inyecta en dbContext en el constructor
      private readonly ILogger<VillaController> _logger;
      private readonly IVillaRepository _iVillaRepository;
      private readonly IMapper _mapper;
      // Constructor
      public VillaController(ILogger<VillaController> logger, IVillaRepository iVillaRepository, IMapper mapper)
      {
         _logger = logger;
         _iVillaRepository = iVillaRepository;
         _mapper = mapper;
      }

      /* GET ALL */
      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
      {
         _logger.LogInformation("Obteniendo todas las villas");
         // traer todas las villas modelo de capa de servicio
         IEnumerable<Villa> response = await _iVillaRepository.GetAll();
         // mapear antes de retornar. Se mapea a un dto
         return Ok(_mapper.Map<IEnumerable<VillaDto>>(response));
      }

      /* GET ONE */
      [HttpGet("{id}", Name = "GetVilla")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]

      public async Task<ActionResult<VillaDto>> GetVilla(int id)
      {
         if (id == 0)
         {
            _logger.LogError("Error al obtener la villa con id {0}", id);
            return BadRequest();
         }
         //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
         var villa = await _iVillaRepository.Get(v => v.Id == id);
         if (villa == null)
         {
            return NotFound();
         }

         return Ok(_mapper.Map<VillaDto>(villa));

      }

      /* CREATE */
      [HttpPost]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status500InternalServerError)]
      public async Task<ActionResult<VillaDto>> CreateVilla([FromBody] VillaCreateDto villaCreateDto)
      {
         // VAlidacion Model State
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         // validacion personalizada
         if (await _iVillaRepository.Get(v => v.Name.ToLower() == villaCreateDto.Nombre.ToLower()) != null)
         {
            ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe.");
            return BadRequest(ModelState);
         }


         // Validación de entrada básica
         if (villaCreateDto == null)
         {
            return BadRequest("El objeto de villa no puede ser nulo.");
         }

         //var nextVillaId = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

         //villaDto.Id = nextVillaId;

         // Almacenamiento de la nueva villa
         //VillaStore.villaList.Add(villaDto);

         // mapear datos del DTO al Modelo
         Villa modelo = _mapper.Map<Villa>(villaCreateDto);
         //{

         //   Name = villaCreateDto.Nombre,
         //   Detalle = villaCreateDto.Detalle,
         //   ImagenUrl = villaCreateDto.ImagenUrl,
         //   Ocupantes = villaCreateDto.Ocupantes,
         //   Tarifa = villaCreateDto.Tarifa,
         //   MetrosCuadrados = villaCreateDto.MetrosCuadrados,
         //   Amenidad = villaCreateDto.Amenidad
         //};

         // dentro del Create esta el save changes
         await _iVillaRepository.Create(modelo);

         // Retorno de la villa creada con código de estado 201 Created
         return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
      }

      /* DELETE */
      [HttpDelete("{id:int}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> Delete(int id)
      {
         if (id == 0)
         {
            return BadRequest();
         }

         var villa = await _iVillaRepository.Get(v => v.Id == id);

         if (villa == null)
         {
            return NotFound();
         }

         await _iVillaRepository.Remove(villa);

         return NoContent();
      }

      /* PUT */
      [HttpPut("{id:int}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaUpdateDto)
      {
         if (villaUpdateDto == null || id != villaUpdateDto.Id)
         {
            return BadRequest("El objeto de villa no puede ser nulo.");
         }

         if (id != villaUpdateDto.Id)
         {
            return BadRequest("El ID proporcionado no coincide con el ID de la villa.");
         }

         // copiar datos del DTO al Modelo
         Villa modelo = _mapper.Map<Villa>(villaUpdateDto);
         //{
         //   Id = updatedVillaDto.Id,
         //   Name = updatedVillaDto.Nombre,
         //   Detalle = updatedVillaDto.Detalle,
         //   ImagenUrl = updatedVillaDto.ImagenUrl,
         //   Ocupantes = updatedVillaDto.Ocupantes,
         //   Tarifa = updatedVillaDto.Tarifa,
         //   MetrosCuadrados = updatedVillaDto.MetrosCuadrados,
         //   Amenidad = updatedVillaDto.Amenidad
         //};

         await _iVillaRepository.Update(modelo);

         // Retorno con código de estado 204 No Content
         return NoContent();
      }

      /* PATCH */
      [HttpPatch("{id:int}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchVillaDto)
      {
         if (patchVillaDto == null || id == 0)
         {
            return BadRequest("El objeto de villa no puede ser nulo ni el id 0");
         }

         // se busca el registro a modificar. Se le indica AsNoTracking
         var existingVilla = await _iVillaRepository.Get(v => v.Id == id, tracked: false);

         if (existingVilla == null)
         {
            return NotFound();
         }

         // se utiliza el paquete para aplicar los cambios. Se le pasa como segundo parametro model state para validar
         //patchVillaDto.ApplyTo(existingVilla, ModelState);

         // copiar los valores de una villa existente al DTO temporal
         VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(existingVilla);
         //{
         //   Id = existingVilla.Id,
         //   Nombre = existingVilla.Name,
         //   Detalle = existingVilla.Detalle,
         //   ImagenUrl = existingVilla.ImagenUrl,
         //   Ocupantes = existingVilla.Ocupantes,
         //   Tarifa = existingVilla.Tarifa,
         //   MetrosCuadrados = existingVilla.MetrosCuadrados,
         //   Amenidad = existingVilla.Amenidad
         //};

         // aplicar el objeto patch a el DTO temporal
         patchVillaDto.ApplyTo(villaDto, ModelState);

         // se pregunta por el model state, para que los cambios sean consistentes
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         // ahora villaDto contiene los cambios aplicados. Pasamos el DTO al modelo temporal para guardar los cambios
         Villa modelo = _mapper.Map<Villa>(villaDto);
         //{
         //   Id = villaDto.Id,
         //   Name = villaDto.Nombre,
         //   Detalle = villaDto.Detalle,
         //   ImagenUrl = villaDto.ImagenUrl,
         //   Ocupantes = villaDto.Ocupantes,
         //   Tarifa = villaDto.Tarifa,
         //   MetrosCuadrados = villaDto.MetrosCuadrados,
         //   Amenidad = villaDto.Amenidad
         //};

         await _iVillaRepository.Update(modelo);


         // Retorno con código de estado 204 No Content
         return NoContent();
      }

   } // class
} // namespace
