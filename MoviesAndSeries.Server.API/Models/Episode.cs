namespace MoviesAndSeries.Server.API.Models
{
	// Represents an episode of a series
	public class Episode : Information
	{
		// The series to which the episode belongs
		public Series Series { get; set; } = new();

		// Foreign key for the series
		public int SeriesId { get; set; }
	}
}