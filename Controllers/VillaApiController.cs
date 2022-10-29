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

        [HttpGet]
        [Route("GetVillaById/{id:int}")]
        public ActionResult<VillaDto> GetVillaById(int id) 
        { 
            if(id == 0)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(it => it.Id == id);

            if (villa == null)
                return NotFound();

            return Ok(villa);    
        }
    }
}
