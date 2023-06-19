namespace MoviesAndSeries.Server.API.Models
{
	public class Information
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public string? Image { get; set; }

		public double? Rating { get; set; }

		public uint Duration { get; set; }
	}
}