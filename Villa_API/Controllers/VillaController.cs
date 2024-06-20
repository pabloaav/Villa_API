using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
      private readonly ApiResponse _response;

      // Constructor
      public VillaController(ILogger<VillaController> logger, IVillaRepository iVillaRepository, IMapper mapper)
      {
         _logger = logger;
         _iVillaRepository = iVillaRepository;
         _mapper = mapper;
         _response = new();
      }

      /* GET ALL */
      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ApiResponse>> GetVillas()
      {
         try
         {
            _logger.LogInformation("Obteniendo todas las villas");
            // traer todas las villas modelo de capa de servicio
            IEnumerable<Villa> villaList = await _iVillaRepository.GetAll();
            // asignar a ApiResponse
            _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
         }
         catch (Exception ex)
         {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
         }

         return BadRequest(_response);
      }

      /* GET ONE */
      [HttpGet("{id}", Name = "GetVilla")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]

      public async Task<ActionResult<ApiResponse>> GetVilla(int id)
      {
         try
         {
            if (id == 0)
            {
               _logger.LogError("Error al obtener la villa con id {0}", id);
               _response.StatusCode = HttpStatusCode.BadRequest;
               return BadRequest(_response);
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _iVillaRepository.Get(v => v.Id == id);
            if (villa == null)
            {
               _response.StatusCode = HttpStatusCode.NotFound;
               _response.IsExitoso = false;
               _response.Resultado = "La villa no fue encontrada.";
               return NotFound(_response);
            }
            _response.Resultado = _mapper.Map<VillaDto>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);

         }
         catch (Exception ex)
         {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
         }

         return BadRequest(_response);

      }

      /* CREATE */
      [HttpPost]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status500InternalServerError)]
      public async Task<ActionResult<ApiResponse>> CreateVilla([FromBody] VillaCreateDto villaCreateDto)
      {
         try
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
               _response.StatusCode = HttpStatusCode.BadRequest;
               _response.IsExitoso = false;
               _response.Resultado = "El objeto de villa no puede ser nulo.";
               return BadRequest(_response);
            }

            // mapear datos del DTO al Modelo
            Villa modelo = _mapper.Map<Villa>(villaCreateDto);

            // dentro del Create esta el save changes
            await _iVillaRepository.Create(modelo);

            _response.IsExitoso = true;
            _response.Resultado = modelo;
            _response.StatusCode = HttpStatusCode.Created;

            // Retorno de la villa creada con código de estado 201 Created
            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _response);
         }
         catch (Exception ex)
         {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
         }

         return BadRequest(_response);
      }

      /* DELETE */
      [HttpDelete("{id:int}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> Delete(int id)
      {
         try
         {
            if (id == 0)
            {
               _response.StatusCode = HttpStatusCode.BadRequest;
               _response.IsExitoso = false;
               _response.Resultado = "El ID de la villa no puede ser 0.";
               return BadRequest();
            }

            var villa = await _iVillaRepository.Get(v => v.Id == id);

            if (villa == null)
            {
               _response.StatusCode = HttpStatusCode.NotFound;
               _response.IsExitoso = false;
               _response.Resultado = "La villa no fue encontrada.";
               return NotFound();
            }

            await _iVillaRepository.Remove(villa);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsExitoso = true;
            _response.Resultado = "La villa fue eliminada.";

            return Ok(_response);
         }
         catch (Exception ex)
         {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
         }

         return BadRequest(_response);
      }

      /* PUT */
      [HttpPut("{id:int}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaUpdateDto)
      {
         if (villaUpdateDto == null)
         {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsExitoso = false;
            _response.Resultado = "El objeto de villa no puede ser nulo.";
            return BadRequest(_response);
         }

         if (id != villaUpdateDto.Id)
         {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsExitoso = false;
            _response.Resultado = "El ID proporcionado no coincide con el ID de la villa.";
            return BadRequest(_response);
         }

         // copiar datos del DTO al Modelo
         Villa modelo = _mapper.Map<Villa>(villaUpdateDto);

         // comprobar existencia del registro
         var existe = await _iVillaRepository.Get(v => v.Id == id, tracked: false);
         if (existe == null)
         {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsExitoso = false;
            _response.Resultado = "La villa no fue encontrada.";
            return NotFound(_response);
         }

         await _iVillaRepository.Update(modelo);

         _response.StatusCode = HttpStatusCode.NoContent;
         _response.IsExitoso = true;
         _response.Resultado = "La villa fue actualizada.";

         // Retorno con código de estado 204 No Content
         return Ok(_response);
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
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsExitoso = false;
            _response.Resultado = "El objeto de villa no puede ser nulo.";
            return BadRequest(_response);
         }

         // se busca el registro a modificar. Se le indica AsNoTracking
         var existingVilla = await _iVillaRepository.Get(v => v.Id == id, tracked: false);

         if (existingVilla == null)
         {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsExitoso = false;
            _response.Resultado = "La villa no fue encontrada.";
            return NotFound(_response);
         }

         // copiar los valores de una villa existente al DTO temporal
         VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(existingVilla);


         // aplicar el objeto patch a el DTO temporal
         patchVillaDto.ApplyTo(villaDto, ModelState);

         // se pregunta por el model state, para que los cambios sean consistentes
         if (!ModelState.IsValid)
         {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsExitoso = false;
            _response.Resultado = ModelState;
            return BadRequest(_response);
         }

         // ahora villaDto contiene los cambios aplicados. Pasamos el DTO al modelo temporal para guardar los cambios
         Villa modelo = _mapper.Map<Villa>(villaDto);

         await _iVillaRepository.Update(modelo);

         _response.StatusCode = HttpStatusCode.NoContent;
         _response.IsExitoso = true;
         _response.Resultado = "La villa fue actualizada.";

         // Retorno con código de estado 204 No Content
         return Ok(_response);
      }

   } // class
} // namespace
