namespace MoviesAndSeries.Server.API.Models
{
	public class Episode : Information
	{
		public Series Series { get; set; } = new();

		public int SeriesId { get; set; }
	}
}