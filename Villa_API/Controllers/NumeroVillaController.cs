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
   public class NumeroVillaController : ControllerBase
   {
      // Para acceder a base de datos se inyecta en dbContext en el constructor
      private readonly ILogger<VillaController> _logger;
      private readonly IVillaRepository _iVillaRepository;
      private readonly INumeroVillaRepository _iNumeroVillaRepository;
      private readonly IMapper _mapper;
      private readonly ApiResponse _response;

      // Constructor
      public NumeroVillaController(ILogger<VillaController> logger, IVillaRepository iVillaRepository, IMapper mapper, INumeroVillaRepository iNumeroVillaRepository)
      {
         _logger = logger;
         _iVillaRepository = iVillaRepository;
         _mapper = mapper;
         _response = new();
         _iNumeroVillaRepository = iNumeroVillaRepository;
      }

      /* GET ALL */
      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ApiResponse>> GetNumeroVillas()
      {
         try
         {
            _logger.LogInformation("Obteniendo todos los números de villa");
            IEnumerable<NumeroVilla> numeroVillaList = await _iNumeroVillaRepository.GetAll();
            _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList);
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
      [HttpGet("{id}", Name = "GetNumeroVilla")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      public async Task<ActionResult<ApiResponse>> GetNumeroVilla(int id)
      {
         try
         {
            if (id == 0)
            {
               _logger.LogError("Error al obtener el número de villa con id {0}", id);
               _response.StatusCode = HttpStatusCode.BadRequest;
               return BadRequest(_response);
            }

            var numeroVilla = await _iNumeroVillaRepository.Get(v => v.VillaNo == id);
            if (numeroVilla == null)
            {
               _response.StatusCode = HttpStatusCode.NotFound;
               _response.IsExitoso = false;
               _response.Resultado = "El número de villa no fue encontrado.";
               return NotFound(_response);
            }

            _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
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
      public async Task<ActionResult<ApiResponse>> CreateNumeroVilla([FromBody] NumeroVillaCreateDto numeroVillaCreateDto)
      {
         try
         {
            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }

            if (await _iNumeroVillaRepository.Get(v => v.VillaNo == numeroVillaCreateDto.VillaNo) != null)
            {
               ModelState.AddModelError("NumeroVillaExiste", "El número de villa ya existe.");
               return BadRequest(ModelState);
            }

            if (numeroVillaCreateDto == null)
            {
               _response.StatusCode = HttpStatusCode.BadRequest;
               _response.IsExitoso = false;
               _response.Resultado = "El objeto de número de villa no puede ser nulo.";
               return BadRequest(_response);
            }

            if (_iVillaRepository.Get(v => v.Id == numeroVillaCreateDto.VillaId) == null)
            {
               ModelState.AddModelError("ClaveForanea", "El Id de la de villa no existe.");
               return BadRequest(ModelState);
            }

            NumeroVilla modelo = _mapper.Map<NumeroVilla>(numeroVillaCreateDto);
            modelo.Created_at = DateTime.Now;

            await _iNumeroVillaRepository.Create(modelo);

            _response.IsExitoso = true;
            _response.Resultado = modelo;
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetNumeroVilla", new { id = modelo.VillaNo }, _response);
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
      public async Task<IActionResult> DeleteNumeroVilla(int id)
      {
         try
         {
            if (id == 0)
            {
               _response.StatusCode = HttpStatusCode.BadRequest;
               _response.IsExitoso = false;
               _response.Resultado = "El ID del número de villa no puede ser 0.";
               return BadRequest();
            }

            var numeroVilla = await _iNumeroVillaRepository.Get(v => v.VillaNo == id);

            if (numeroVilla == null)
            {
               _response.StatusCode = HttpStatusCode.NotFound;
               _response.IsExitoso = false;
               _response.Resultado = "El número de villa no fue encontrado.";
               return NotFound();
            }

            await _iNumeroVillaRepository.Remove(numeroVilla);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsExitoso = true;
            _response.Resultado = "El número de villa fue eliminado.";

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
      public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto numeroVillaUpdateDto)
      {
         if (numeroVillaUpdateDto == null)
         {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsExitoso = false;
            _response.Resultado = "El objeto de número de villa no puede ser nulo.";
            return BadRequest(_response);
         }

         if (id != numeroVillaUpdateDto.VillaNo)
         {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsExitoso = false;
            _response.Resultado = "El ID proporcionado no coincide con el ID del número de villa.";
            return BadRequest(_response);
         }

         NumeroVilla modelo = _mapper.Map<NumeroVilla>(numeroVillaUpdateDto);

         var existe = await _iNumeroVillaRepository.Get(v => v.VillaNo == id, tracked: false);
         if (existe == null)
         {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsExitoso = false;
            _response.Resultado = "El número de villa no fue encontrado.";
            return NotFound(_response);
         }

         // controlar el id de la villa
         if (_iVillaRepository.Get(v => v.Id == numeroVillaUpdateDto.VillaId) == null)
         {
            ModelState.AddModelError("ClaveForanea", "El Id de la de villa no existe.");
            return BadRequest(ModelState);
         }

         await _iNumeroVillaRepository.Update(modelo);

         _response.StatusCode = HttpStatusCode.NoContent;
         _response.IsExitoso = true;
         _response.Resultado = "El número de villa fue actualizado.";

         return Ok(_response);
      }


      /* PATCH */
      [HttpPatch("{id:int}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> UpdatePartialNumeroVilla(int id, JsonPatchDocument<NumeroVillaUpdateDto> patchNumeroVillaDto)
      {
         if (patchNumeroVillaDto == null || id == 0)
         {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsExitoso = false;
            _response.Resultado = "El objeto de número de villa no puede ser nulo.";
            return BadRequest(_response);
         }

         var existingNumeroVilla = await _iNumeroVillaRepository.Get(v => v.VillaNo == id, tracked: false);

         if (existingNumeroVilla == null)
         {
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.IsExitoso = false;
            _response.Resultado = "El número de villa no fue encontrado.";
            return NotFound(_response);
         }

         NumeroVillaUpdateDto numeroVillaDto = _mapper.Map<NumeroVillaUpdateDto>(existingNumeroVilla);

         patchNumeroVillaDto.ApplyTo(numeroVillaDto, ModelState);

         if (!ModelState.IsValid)
         {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsExitoso = false;
            _response.Resultado = ModelState;
            return BadRequest(_response);
         }

         NumeroVilla modelo = _mapper.Map<NumeroVilla>(numeroVillaDto);

         await _iNumeroVillaRepository.Update(modelo);

         _response.StatusCode = HttpStatusCode.NoContent;
         _response.IsExitoso = true;
         _response.Resultado = "El número de villa fue parcialmente actualizado.";

         return Ok(_response);
      }

   } // class
} // namespace
