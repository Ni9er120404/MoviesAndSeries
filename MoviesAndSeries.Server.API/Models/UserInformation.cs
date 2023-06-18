namespace MoviesAndSeries.Server.API.Models
{
	public class UserInformation
	{
		public Guid Id { get; set; }

		public string UserName { get; set; } = null!;

		public string Email { get; set; } = null!;

		public string Password { get; set; } = null!;
	}
}