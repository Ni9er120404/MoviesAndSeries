namespace MoviesAndSeries.Server.API.Models
{
	// Represents a movie
	public class Movie : Information
	{
		// The user who added the movie
		public User User { get; set; } = new();

		// Foreign key for the user
		public Guid UserId { get; set; }
	}
}