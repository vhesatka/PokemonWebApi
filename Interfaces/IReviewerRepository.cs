using Assignment1.Models;

namespace Assignment1.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int id);
        bool ReviewerExists(int reviewerId);
        bool CreateReviewer(Reviewer createReviewer);
        bool Saved();
    }
}
