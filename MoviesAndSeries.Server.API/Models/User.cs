namespace MoviesAndSeries.Server.API.Models
{
	public class User
	{
		public Guid Id { get; set; }

		public string UserName { get; set; } = null!;

		public string Email { get; set; } = null!;

		public string Password { get; set; } = null!;

		public ICollection<Series> Series { get; set; } = new List<Series>();

		public ICollection<Movie>? Movies { get; set; } = new List<Movie>();
	}
}