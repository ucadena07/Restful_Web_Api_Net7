using MagicVillaApi.Data;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        [HttpGet]
        [Route("GetVillas")]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDto> GetVilla(int id) 
        { 
            if(id == 0)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(it => it.Id == id);

            if (villa == null)
                return NotFound();

            return Ok(villa);    
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody]VillaDto villaDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            };
            if(villaDto == null) return BadRequest();
            if(villaDto.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            villaDto.Id = VillaStore.villaList.OrderByDescending(it => it.Id).FirstOrDefault().Id + 1;  
            VillaStore.villaList.Add(villaDto);
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id}, villaDto);
        }
    }
}
