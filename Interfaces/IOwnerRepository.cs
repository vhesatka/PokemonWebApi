using Assignment1.Models;
using Microsoft.AspNetCore.Components.Web;

namespace Assignment1.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        ICollection<Owner> GetOwnersOfAPokemon(int pokeId);
        bool OwnerExists(int ownerId);
        bool CreateOwner(Owner createOwner);
        bool Saved();
    }
}
