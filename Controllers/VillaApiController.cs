using AutoMapper;
using MagicVillaApi.Data;
using MagicVillaApi.Logging;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;
using MagicVillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        public VillaApiController(IVillaRepository villaRepository, IMapper mapper)
        {
            _villaRepository = villaRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetVillas")]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            var villas = await _villaRepository.GetAll();
            return Ok(_mapper.Map<List<VillaDto>>(villas));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {

                return BadRequest();
            }

            var villa = await _villaRepository.Get(it => it.Id == id);

            if (villa == null)
                return NotFound();

            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CreateVilla([FromBody] VillaCreateDto createDto)
        {
            //if (!ModelState.IsValid) {
            //    return BadRequest(ModelState);
            //};
            if (await _villaRepository.Get(it => it.Name.ToLower() == createDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exist");
                return BadRequest(ModelState);
            }

            if (createDto == null) return BadRequest();


            Villa model = _mapper.Map<Villa>(createDto);

            await _villaRepository.Create(model);

            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            var villa = await _villaRepository.Get(it => it.Id == id);
            if (villa == null) return NotFound();

            await _villaRepository.Remove(villa);

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id) return BadRequest();
            var villa = await _villaRepository.Get(it => it.Id == id,tracked:false);
            if (villa == null) return NotFound();

            Villa model = _mapper.Map<Villa>(updateDto);

            await _villaRepository.Update(model);
            await _villaRepository.Save();

            return NoContent();

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<VillaUpdateDto> patchDto)
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
            return NoContent();

        }

    }
}
