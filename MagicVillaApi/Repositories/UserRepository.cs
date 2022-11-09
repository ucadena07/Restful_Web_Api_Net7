using AutoMapper;
using MagicVillaApi.Data;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;
using MagicVillaApi.Repositories.IRepositories;

namespace MagicVillaApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private string secretKey;
  
        public UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration config)
        {
            _db = db;
            _mapper = mapper;
            secretKey = config.GetValue<string>("ApiSettings:JwtSecretKey");
        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(it => it.UserName == username);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.LocalUsers.FirstOrDefault(it => it.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
            && it.Password == loginRequestDTO.Password);

            if(user == null)
            {
                return null;
            }

            //generate Jwt token


        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDto)
        {
            LocalUser user = new()
            {
                UserName = registrationRequestDto.UserName,
                Password= registrationRequestDto.Password,
                Name = registrationRequestDto.Name,
                Role = registrationRequestDto.Role
            };

            _db.LocalUsers.Add(user);
            await _db.SaveChangesAsync();

            user.Password = "";
            return user;
        }
    }
}
