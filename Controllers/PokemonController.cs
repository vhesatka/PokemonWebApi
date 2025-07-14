using Assignment1.Dto;
using Assignment1.Interfaces;
using Assignment1.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private ICategoryRepository _categoryRepository;
        private IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, ICategoryRepository categoryRepository, IOwnerRepository ownerRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _categoryRepository = categoryRepository;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]

        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));

            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            return Ok(rating);  
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDto createPokemon)
        {
            if(createPokemon == null)
            {
                return BadRequest(ModelState);
            }

            var pokeExists = _pokemonRepository.GetPokemons().Where(p => p.Name.Trim().ToUpper() == createPokemon.Name.Trim().ToUpper()).FirstOrDefault();
            
            if(pokeExists != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return BadRequest(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(createPokemon);

            if (!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return BadRequest(ModelState);
            }

            return Ok("Saved successfully");
        }

    }
}
