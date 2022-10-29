using MagicVillaApi.Models;
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
        public IEnumerable<Villa> GetVillas()
        {
            return new List<Villa>()
            {
                new Villa() {Id = 1, Name = "Villa 1"},
                new Villa() {Id = 2, Name = "Villa 2"}
            };
        }
    }
}
