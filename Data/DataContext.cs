using Assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment1.Data
{
    public class DataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Country> Countries { get; set; }
        public DbSet<Models.Owner> Owners { get; set; }
        public DbSet<Models.Pokemon> Pokemons { get; set; }
        public DbSet<Models.PokemonOwner> PokemonOwners { get; set; }
        public DbSet<Models.PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Models.Review> Reviews { get; set; }
        public DbSet<Models.Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(pc => new { pc.PokemonId, pc.CategoryId });
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<PokemonOwner>()
                .HasKey(po => new { po.PokemonId, po.OwnerId });
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Owner)
                .WithMany(pc => pc.PokemonOwners)
                .HasForeignKey(c => c.OwnerId);
        }
    }
}
