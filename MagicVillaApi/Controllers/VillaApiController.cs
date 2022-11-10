using AutoMapper;
using Azure;
using MagicVillaApi.Data;
using MagicVillaApi.Logging;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;
using MagicVillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        private APIResponse _response;
        public VillaApiController(IVillaRepository villaRepository, IMapper mapper)
        {
            _villaRepository = villaRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                var villas = await _villaRepository.GetAll();
                _response.Result = _mapper.Map<List<VillaDto>>(villas);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {


                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRepository.Get(it => it.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound();
                }
     


                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto createDto)
        {
            //if (!ModelState.IsValid) {
            //    return BadRequest(ModelState);
            //};
            try
            {
                if (await _villaRepository.Get(it => it.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa already exist");
                    return BadRequest(ModelState);
                }

                if (createDto == null) return BadRequest();


                Villa model = _mapper.Map<Villa>(createDto);

                await _villaRepository.Create(model);

                _response.Result = _mapper.Map<VillaDto>(model);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "custom")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0) return BadRequest();
                var villa = await _villaRepository.Get(it => it.Id == id);
                if (villa == null) return NotFound();

                await _villaRepository.Remove(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id) return BadRequest();
                var villa = await _villaRepository.Get(it => it.Id == id, tracked: false);
                if (villa == null) return NotFound();

                Villa model = _mapper.Map<Villa>(updateDto);

                await _villaRepository.Update(model);
                await _villaRepository.Save();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0) return BadRequest();
                var villa = await _villaRepository.Get(it => it.Id == id, tracked: false);
                if (villa == null) return NotFound();

                VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

                patchDto.ApplyTo(villaDto, ModelState);

                if (!ModelState.IsValid) return BadRequest(ModelState);

                Villa model = _mapper.Map<Villa>(villaDto);

                await _villaRepository.Update(model);
                await _villaRepository.Save();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;

        }

    }
}
