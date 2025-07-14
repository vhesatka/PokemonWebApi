using Assignment1.Data;
using Assignment1.Interfaces;
using Assignment1.Models;

namespace Assignment1.Repositories
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;
        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReviewer(Reviewer createReviewer)
        {
            _context.Add(createReviewer);
            return Saved();
        }

        public Reviewer GetReviewer(int id)
        {
            return _context.Reviewers.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool Saved()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
