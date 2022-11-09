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
            throw new NotImplementedException();
        }

        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
