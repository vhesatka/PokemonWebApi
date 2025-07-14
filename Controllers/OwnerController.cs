using Assignment1.Dto;
using Assignment1.Interfaces;
using Assignment1.Models;
using Assignment1.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();
            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemons")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}/owners")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersOfAPokemon(int pokeId)
        {

            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnersOfAPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto createOwner)
        {
            if (createOwner == null)
            {
                return BadRequest(ModelState);
            }

            var firstNameExists = _ownerRepository.GetOwners().Where(p => p.FirstName.Trim().ToUpper() == createOwner.FirstName.Trim().ToUpper()).FirstOrDefault();
            var lastNameExists = _ownerRepository.GetOwners().Where(p => p.LastName.Trim().ToUpper() == createOwner.LastName.Trim().ToUpper()).FirstOrDefault();

            if (firstNameExists != null && lastNameExists != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(createOwner);

            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return BadRequest(ModelState);
            }

            return Ok("Saved successfully");
        }
    }
}
