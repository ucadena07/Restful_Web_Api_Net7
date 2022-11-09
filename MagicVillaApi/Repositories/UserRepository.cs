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
        public UserRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

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
            throw new NotImplementedException();
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
