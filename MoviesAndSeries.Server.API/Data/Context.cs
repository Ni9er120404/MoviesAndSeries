using Microsoft.EntityFrameworkCore;
using MoviesAndSeries.Server.API.Models;

namespace MoviesAndSeries.Server.API.Data
{
	public class Context : DbContext
	{
		public Context(DbContextOptions<Context> options) : base(options)
		{
			_ = Database.EnsureDeleted();
			_ = Database.EnsureCreated();
		}


		public DbSet<User>? Users { get; set; }

		public DbSet<Movie>? Movies { get; set; }

		public DbSet<Series>? Series { get; set; }

		public DbSet<Episode>? Episodes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			_ = modelBuilder.Entity<Series>()
				.HasOne(s => s.User)
				.WithMany(u => u.Series)
				.HasForeignKey(s => s.UserId);

			_ = modelBuilder.Entity<Episode>()
				.HasOne(e => e.Series)
				.WithMany(s => s.Episodes)
				.HasForeignKey(e => e.SeriesId);

			_ = modelBuilder.Entity<Movie>()
				.HasOne(m => m.User)
				.WithMany(u => u.Movies)
				.HasForeignKey(m => m.UserId);
		}
	}
}