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
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto createReviewer)
        {
            if (createReviewer == null)
                return BadRequest(ModelState);

            var firstNameExists = _reviewerRepository.GetReviewers().Where(r => r.FirstName.Trim().ToUpper() == createReviewer.FirstName.Trim().ToUpper()).FirstOrDefault();
            var lastNameExists = _reviewerRepository.GetReviewers().Where(r => r.LastName.Trim().ToUpper() == createReviewer.LastName.Trim().ToUpper()).FirstOrDefault();

            if (firstNameExists != null && lastNameExists != null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return BadRequest(ModelState);
            }

            var reviewerMap = _mapper.Map<Reviewer>(createReviewer);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return BadRequest(ModelState);
            }

            return Ok("Saved successfully");
        }
    }
}
