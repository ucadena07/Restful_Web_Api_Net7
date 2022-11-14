using MagicVillaApi.Helpers.Interfaces;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;
using MagicVillaApi.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Net;

namespace MagicVillaApi.Controllers
{
    [Route("api/v{version:apiVersion}/UsersAuth")]
    [ApiController]
    [ApiVersionNeutral]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private APIResponse _response;

        public UsersController(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _response = new();
            _passwordService = passwordService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);
            if(loginResponse.User is null || string.IsNullOrEmpty(loginResponse.Token)) 
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            bool isUniqueUser =  _userRepository.IsUniqueUser(model.UserName);
            if(!isUniqueUser)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }
            var user = await _userRepository.Register(model);
            if(user is null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error during registration process");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        //[HttpPost("registerv2")]
        //public async Task<IActionResult> RegisterV2([FromBody] RegistrationRequestDTO model)
        //{
        //    var pass = _passwordService.CreatePasswordHash(model.Password);

        //    UserV2 user = new()
        //    {
        //        Username = model.UserName,
        //        PasswordHash= pass.passwordHash,
        //        PasswordSalt = pass.passwordSalt
        //    };

        //    _response.Result = user;
        //    _response.StatusCode = HttpStatusCode.OK;
        //    _response.IsSuccess = true;
        //    return Ok(_response);
        //}

        //[HttpPost("loginv2")]
        //public async Task<IActionResult> LoginV2([FromBody] LoginRequestDTO model)
        //{
        //    var pass = _passwordService.CreatePasswordHash(model.Password);

        //    UserV2 user = new()
        //    {
        //        Username = model.UserName,
        //        PasswordHash = pass.passwordHash,
        //        PasswordSalt = pass.passwordSalt
        //    };

        //    var checkPassword = _passwordService.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt);


        //    _response.StatusCode = HttpStatusCode.OK;
        //    _response.IsSuccess = true;
        //    //_response.Result = loginResponse;
        //    return Ok(_response);
        //}
    }
}
