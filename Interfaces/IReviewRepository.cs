using Assignment1.Models;

namespace Assignment1.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        ICollection<Review> GetReviewsOfAPokemon(int pokeId);
        bool ReviewExists(int id);
        bool CreateReview(int pokeId, int reviewerId, Review createReview);
        bool Saved();
    }
}
