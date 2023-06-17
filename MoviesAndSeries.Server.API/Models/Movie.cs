namespace MoviesAndSeries.Server.API.Models
{
	public class Movie : Information
	{
		public User User { get; set; } = new();

		public Guid UserId { get; set; }
	}
}