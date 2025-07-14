using Assignment1.Data;
using Assignment1.Interfaces;
using Assignment1.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Assignment1.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(o => o.OwnerId == ownerId).Select(p => p.Pokemon).ToList();
        }

        public ICollection<Owner> GetOwnersOfAPokemon(int pokeId)
        {
            return _context.PokemonOwners.Where(p => p.PokemonId == pokeId).Select(o => o.Owner).ToList();
        }

        public bool CreateOwner(Owner createOwner)
        {
            _context.Add(createOwner);
            return Saved();
        }

        public bool Saved()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }

}
