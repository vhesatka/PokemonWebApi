using Assignment1.Data;
using Assignment1.Interfaces;
using Assignment1.Models;

namespace Assignment1.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemons.Any(p => p.Id == pokeId);
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(p => p.Id == pokeId);

            if(review.Count() <= 0)
            {
                return 0;
            }

            return ((decimal)review.Sum(r => r.Rating)/review.Count());
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon createPokemon)
        {
            var pokemonOwnerEntity = _context.Owners.Where(a => a.Id == ownerId).FirstOrDefault();
            var category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = createPokemon,
            };
            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = createPokemon
            };
            _context.Add(pokemonCategory);

            _context.Add(createPokemon);
            return Saved();
        }

        public bool Saved()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
