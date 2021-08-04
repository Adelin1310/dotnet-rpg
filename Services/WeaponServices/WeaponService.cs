using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponServices
{
    public class WeaponService : IWeaponService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(x => x.CharacterId == newWeapon.CharacterId &&
                    x.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                if (character != null)
                {
                    var weapon = new Weapon
                    {
                        Name = newWeapon.Name,
                        Damage = newWeapon.Damage,
                        Character = character
                    };

                    _context.Weapons.Add(weapon);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                    serviceResponse.Message = "Weapon added successfully";
                    serviceResponse.Success = true;
                }
                else
                {
                    serviceResponse.Data = null;
                    serviceResponse.Message = "Character not found!";
                    serviceResponse.Success = false;
                }
            }
            catch (System.Exception ex)
            {
                serviceResponse.Data = null;
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false; 
            }
            return serviceResponse;
        }
    }
}