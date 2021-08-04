using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Models;
using dotnet_rpg.Dtos;
using AutoMapper;
using dotnet_rpg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace dotnet_rpg.Services.CharacterServices
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(x => x.Id == GetUserID());
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>
            {
                Data = await _context.Characters
                .Where(x => x.User.Id == GetUserID())
                .Select(x => _mapper.Map<GetCharacterDto>(x)).ToListAsync()
            };
            return ServiceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
            if (await _context.Characters.CountAsync() > 0)
            {
                try
                {
                    var ch = await _context.Characters.FirstOrDefaultAsync(x => x.CharacterId == id && x.User.Id == GetUserID());
                    if (ch != null)
                    {
                        _context.Characters.Remove(ch);
                        await _context.SaveChangesAsync();

                        ServiceResponse.Data = _mapper.Map<List<GetCharacterDto>>(await _context.Characters
                        .Where(x => x.User.Id == GetUserID())
                        .ToListAsync());
                        ServiceResponse.Message = "Character erased succesfully!";
                        ServiceResponse.Success = true;
                    }
                    else
                    {
                        ServiceResponse.Message = "Character not found!";
                        ServiceResponse.Success = false;
                    }
                }
                catch (System.Exception ex)
                {
                    ServiceResponse.Message = "Character not found! Make sure the entered ID is right!";
                    ServiceResponse.Success = false;
                }
            }
            else
            {
                ServiceResponse.Message = "There are no characters for this user!";
                ServiceResponse.Success = false;
            }

            return ServiceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var dbChars = await _context.Characters.Where(x => x.User.Id == GetUserID()).ToListAsync();
            var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>
            {
                Data = dbChars.Select(x => _mapper.Map<GetCharacterDto>(x)).ToList()
            };
            return ServiceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var dbChar = await _context.Characters.FirstOrDefaultAsync(x => x.CharacterId == id && x.User.Id == GetUserID());
            return new ServiceResponse<GetCharacterDto> { Data = _mapper.Map<GetCharacterDto>(dbChar) };
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var ServiceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {

                Character character = await _context.Characters
                    .Include(x=>x.User)
                    .FirstOrDefaultAsync(x => x.CharacterId == updatedCharacter.CharacterId);
                if (character.User.Id == GetUserID())
                {
                    character.Name = updatedCharacter.Name;
                    character.Class = updatedCharacter.Class;
                    character.Strength = updatedCharacter.Strength;
                    character.Mana = updatedCharacter.Mana;
                    character.Agility = updatedCharacter.Agility;
                    character.HitPoints = updatedCharacter.HitPoints;
                    character.Intelligence = updatedCharacter.Intelligence;

                    await _context.SaveChangesAsync();

                    ServiceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                }
                else
                {
                    ServiceResponse.Message = "Character not found! Make sure the entered ID is right!";
                    ServiceResponse.Success = false;
                }
            }
            catch (System.NullReferenceException ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }

            return ServiceResponse;
        }

        private int GetUserID() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}