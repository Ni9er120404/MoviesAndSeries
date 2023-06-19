using Microsoft.AspNetCore.Mvc;
using MoviesAndSeries.Server.API.Data;
using MoviesAndSeries.Server.API.Models;

namespace MoviesAndSeries.Server.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly Context _context;

		public UserController(Context context)
		{
			_context = context;
		}

		[HttpGet("{id}", Name = nameof(GetUser))]
		public ActionResult GetUser(Guid id)
		{
			User? user = _context.Users!.FirstOrDefault(u => u.Id == id);

			if (user is not null)
			{
				return Ok();
			}
			else
			{
				return NotFound("User not found");
			}
		}

		[HttpPost(Name = nameof(UserRegistrationAsync))]
		public async Task<ActionResult> UserRegistrationAsync(UserInformation userInformation)
		{
			if (userInformation is not null)
			{
				User user = new()
				{
					Id = userInformation.Id,
					UserName = userInformation.UserName,
					Password = userInformation.Password,
					Email = userInformation.Email
				};

				_ = await _context.Users!.AddAsync(user);
				_ = await _context.SaveChangesAsync();

				return Ok("A new user has been created");
			}
			else
			{
				return BadRequest("User creation error");
			}
		}

		[HttpDelete("{id}", Name = nameof(DeleteUser))]
		public async Task<ActionResult> DeleteUser(Guid id)
		{
			User? user = _context.Users!.FirstOrDefault(u => u.Id == id);

			if (user is not null)
			{
				_ = _context.Users!.Remove(user);

				_ = await _context.SaveChangesAsync();

				return Ok("The user has been deleted");
			}
			else
			{
				return NotFound("User not found");
			}
		}
	}
}