using Microsoft.EntityFrameworkCore;
using MoviesAndSeries.Server.API.Models;

namespace MoviesAndSeries.Server.API.Data
{
	// Represents the database context
	public class Context : DbContext
	{
		public Context(DbContextOptions<Context> options) : base(options)
		{
			// Ensures that the database is deleted and created on each context initialization
			_ = Database.EnsureDeleted();
			_ = Database.EnsureCreated();
		}

		// Represents the users table in the database
		public DbSet<User>? Users { get; set; }

		// Represents the movies table in the database
		public DbSet<Movie>? Movies { get; set; }

		// Represents the series table in the database
		public DbSet<Series>? Series { get; set; }

		// Represents the episodes table in the database
		public DbSet<Episode>? Episodes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Configure the relationship between series and user
			_ = modelBuilder.Entity<Series>()
				.HasOne(s => s.User)
				.WithMany(u => u.Series)
				.HasForeignKey(s => s.UserId);

			// Configure the relationship between episodes and series
			_ = modelBuilder.Entity<Episode>()
				.HasOne(e => e.Series)
				.WithMany(s => s.Episodes)
				.HasForeignKey(e => e.SeriesId);

			// Configure the relationship between movies and user
			_ = modelBuilder.Entity<Movie>()
				.HasOne(m => m.User)
				.WithMany(u => u.Movies)
				.HasForeignKey(m => m.UserId);
		}
	}
}