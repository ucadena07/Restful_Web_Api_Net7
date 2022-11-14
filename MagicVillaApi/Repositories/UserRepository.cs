using AutoMapper;
using MagicVillaApi.Data;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;
using MagicVillaApi.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace MagicVillaApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IMapper _mapper;
        private string secretKey;
  
        public UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration config, UserManager<ApplicationUser> userManagaer, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            secretKey = config.GetValue<string>("ApiSettings:JwtSecretKey");
            _userManager = userManagaer;
            _roleManager = roleManager;
        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(it => it.UserName == username);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(it => it.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if(user is null || isValid is false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            //generate Jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var roles = await _userManager.GetRolesAsync(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("name", user.Name.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
    
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO response = new LoginResponseDTO
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),
                //Role = roles.FirstOrDefault()

            };

            return response;

        }

        public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.UserName,
                Email = registrationRequestDto.Name,
                Name = registrationRequestDto.Name,
                NormalizedEmail = registrationRequestDto.Name.ToUpper()
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if(result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(user, "admin");
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(it => it.UserName == registrationRequestDto.UserName);
                    return _mapper.Map<UserDTO>(userToReturn);  
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return new UserDTO();

        }
    }
}
