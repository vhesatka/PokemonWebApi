using Assignment1.Data;
using Assignment1.Interfaces;
using Assignment1.Models;

namespace Assignment1.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReview(int pokeId, int reviewerId, Review createReview)
        {
            _context.Add(createReview);
            return Saved();

        }

        public Review GetReview(int id)
        {
            return _context.Reviews.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _context.Reviewers.Where(r => r.Id == reviewerId).SelectMany(c => c.Reviews).ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return _context.Pokemons.Where(p => p.Id == pokeId).SelectMany(c => c.Reviews).ToList();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(r => r.Id == id);
        }

        public bool Saved()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
