namespace MoviesAndSeries.Server.API.Models
{
	// Represents user information
	public class UserInformation
	{
		// The unique identifier for the user
		public Guid Id { get; set; }

		// The username of the user
		public string UserName { get; set; } = null!;

		// The email of the user
		public string Email { get; set; } = null!;

		// The password of the user
		public string Password { get; set; } = null!;
	}
}