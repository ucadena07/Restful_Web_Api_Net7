using MagicVillaApi.Data;
using MagicVillaApi.Logging;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        //private readonly ILogger<VillaApiController> _logger;
        //private readonly ILogging _logger;
        public VillaApiController(ILogging logger)
        {
   
        }

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
            {

                return BadRequest();
            }

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
            //if (!ModelState.IsValid) {
            //    return BadRequest(ModelState);
            //};
            if(villaDto == null) return BadRequest();
            if(villaDto.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            villaDto.Id = VillaStore.villaList.OrderByDescending(it => it.Id).FirstOrDefault().Id + 1;  
            VillaStore.villaList.Add(villaDto);
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id}, villaDto);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id) 
        { 
            if(id== 0) return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(it => it.Id == id);
            if (villa == null) return NotFound();

            VillaStore.villaList.Remove(villa); 

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDto villaDto)
        {
            if(villaDto == null || id != villaDto.Id) return BadRequest();   
            var villa = VillaStore.villaList.FirstOrDefault(it => it.Id == id); 
            if (villa == null) return NotFound();

            villa.Name = villaDto.Name;
            villa.Occupancy = villaDto.Occupancy;
            villa.Sqft = villaDto.Sqft;

            return NoContent();

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<VillaDto> patchDto)
        {
            if (patchDto == null || id == 0) return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(it => it.Id == id);
            if (villa == null) return NotFound();

            patchDto.ApplyTo(villa, ModelState);

            if(!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();

        }

    }
}
