using Assignment1.Data;
using Assignment1.Dto;
using Assignment1.Interfaces;
using Assignment1.Models;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("owners/{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersByCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();
            var owners = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnersByCountry(countryId));

            if (owners.Count() <= 0)
                return NotFound("no owner from this country");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateCountry([FromBody] CountryDto createCountry)
        {
            if(createCountry == null)
            {
                return BadRequest(ModelState);
            }

            var country = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == createCountry.Name.Trim().ToUpper()).FirstOrDefault();
            if(country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(412, ModelState);
            }

            var countryMap = _mapper.Map<Country>(createCountry);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Country saved successfully");
        }

    }
}
