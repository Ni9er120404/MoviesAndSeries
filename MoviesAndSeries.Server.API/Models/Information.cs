namespace MoviesAndSeries.Server.API.Models
{
	// Represents common information shared by movies and series
	public class Information
	{
		// The unique identifier for the information
		public int Id { get; set; }

		// The name of the movie or series
		public string Name { get; set; } = null!;

		// The description of the movie or series
		public string? Description { get; set; }

		// The image URL of the movie or series
		public string? Image { get; set; }

		// The rating of the movie or series
		public double? Rating { get; set; }

		// The duration of the movie or series
		public uint Duration { get; set; }
	}
}