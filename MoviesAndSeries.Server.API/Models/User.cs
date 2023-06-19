namespace MoviesAndSeries.Server.API.Models
{
	// Represents a user
	public class User
	{
		// The unique identifier for the user
		public Guid Id { get; set; }

		// The username of the user
		public string UserName { get; set; } = null!;

		// The email of the user
		public string Email { get; set; } = null!;

		// The password of the user
		public string Password { get; set; } = null!;

		// The series added by the user
		public ICollection<Series> Series { get; set; } = new List<Series>();

		// The movies added by the user
		public ICollection<Movie>? Movies { get; set; } = new List<Movie>();
	}
}