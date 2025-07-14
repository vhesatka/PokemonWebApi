using Assignment1.Dto;
using Assignment1.Interfaces;
using Assignment1.Models;
using Assignment1.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IPokemonRepository pokemonRepository, IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            var review = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);

        }

        [HttpGet("{pokeId}/reviewsofapokemon/")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfAPokemon(int pokeId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateReview([FromQuery] int pokeId, [FromQuery] int reviewerId, [FromBody] ReviewDto createReview)
        {
            if (createReview == null)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                ModelState.AddModelError("", "Pokemon does not exist");
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                ModelState.AddModelError("", "Reviewer does not exist");
                return BadRequest(ModelState);
            }

            

            var reviewMap = _mapper.Map<Review>(createReview);

            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);

            if (!_reviewRepository.CreateReview(pokeId, reviewerId, reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return BadRequest(ModelState);
            }

            return Ok("Saved successfully");
        }



    }
}
