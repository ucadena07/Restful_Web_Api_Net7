using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _baseUrl;
        public AuthService(IHttpClientFactory clientFactory, IConfiguration config) : base(clientFactory)
        {
            _httpClientFactory = clientFactory;
            _baseUrl = config.GetValue<string>("ServicesUrls:VillaAPI");
        }
        public Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = _baseUrl + "/api/UsersAuth/login",
            });
        }

        public Task<T> RegisterAsync<T>(RegistrationRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = _baseUrl + "/api/UsersAuth/register",
            });
        }
    }
}
