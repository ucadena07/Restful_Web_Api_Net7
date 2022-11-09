using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;

namespace MagicVillaApi.Repositories.IRepositories
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username); 
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDto);
    }
}
