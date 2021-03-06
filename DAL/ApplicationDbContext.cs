using System;
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Ship> Ships { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<BoardState> BoardStates { get; set; } = null!;
        public DbSet<Board> Boards { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder
                .Entity<Player>()
                .HasOne<Game>()
                .WithOne(x => x.FirstPlayer)
                .HasForeignKey<Game>(x => x.FirstPlayerId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder
                .Entity<Player>()
                .HasOne<Game>()
                .WithOne(x => x.SecondPlayer)
                .HasForeignKey<Game>(x => x.SecondPlayerId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder
                .Entity<Board>()
                .HasOne<Player>()
                .WithOne(x => x.Board)
                .HasForeignKey<Player>(x => x.BoardId)
                .OnDelete(DeleteBehavior.NoAction);
            
        }


        //Remove the cascade delete.
            /*foreach (var relationship in modelBuilder.Model
                .GetEntityTypes()
                .Where(e => !e.IsOwned())
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }*/
        }
}