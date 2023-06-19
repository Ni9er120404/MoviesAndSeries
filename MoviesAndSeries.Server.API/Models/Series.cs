using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAndSeries.Server.API.Models
{
	public class Series : Information
	{
		public ICollection<Episode>? Episodes { get; set; } = new List<Episode>();

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

		[NotMapped]
		public string? PosterId { get; set; }

		public ushort? StartDate { get; set; }

		public ushort? EndDate { get; set; }

		public User User { get; set; } = new();

		public Guid UserId { get; set; }
	}
}