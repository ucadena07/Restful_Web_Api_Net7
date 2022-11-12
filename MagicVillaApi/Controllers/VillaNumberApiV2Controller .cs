using AutoMapper;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;
using MagicVillaApi.Repositories.IRepositories;
using MagicVillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVillaApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberApiV2Controller : ControllerBase
    {
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        private APIResponse _response;
        public VillaNumberApiV2Controller(IVillaNumberRepository dbVillaNumber, IMapper mapper, IVillaRepository villaRepository)
        {
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            _response = new();
            _villaRepository = villaRepository;
        }

    

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public List<string> GetVillaNumbers()
        {
            return new List<string>() { "value1", "value2" };
           

        }


    }
}
