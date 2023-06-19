using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAndSeries.Server.API.Models
{
	// Represents a series
	public class Series : Information
	{
		// The episodes of the series
		public ICollection<Episode>? Episodes { get; set; } = new List<Episode>();

		// The total duration of all episodes in the series
		public uint TotalDuration
		{
			get
			{
				uint totalDuration = 0;

				foreach (Episode? episode in Episodes!.Where(episode => episode is not null))
				{
					totalDuration += episode.Duration;
				}

				return totalDuration;
			}
		}

		// Not mapped to the database - used for holding poster image ID
		[NotMapped]
		public string? PosterId { get; set; }

		// The start year of the series
		public ushort? StartDate { get; set; }

		// The end year of the series
		public ushort? EndDate { get; set; }

		// The user who added the series
		public User User { get; set; } = new();

		// Foreign key for the user
		public Guid UserId { get; set; }
	}
}