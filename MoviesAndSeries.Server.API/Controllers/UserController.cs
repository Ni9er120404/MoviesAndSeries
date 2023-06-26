using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAndSeries.Server.API.Data;
using MoviesAndSeries.Server.API.Models;

namespace MoviesAndSeries.Server.API.Controllers
{
    // Controller for managing user-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Context _context;

        public UserController(Context context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ICollection<Series>> GetAllSeries()
        {
            List<Series> series = await _context.Series!.ToListAsync();

            _ = await _context.SaveChangesAsync();

            return series;
        }

        // GET api/user/{id}
        [HttpGet("{id}", Name = nameof(GetUser))]
        public ActionResult GetUser(Guid id)
        {
            // Retrieve the user with the specified id from the database
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

        // POST api/user
        [HttpPost(Name = nameof(UserRegistrationAsync))]
        public async Task<ActionResult> UserRegistrationAsync(UserInformation userInformation)
        {
            if (userInformation is not null)
            {
                // Create a new user object with the provided information
                User user = new()
                {
                    Id = userInformation.Id,
                    UserName = userInformation.UserName,
                    Password = userInformation.Password,
                    Email = userInformation.Email
                };

                // Add the user to the database and save changes
                _ = await _context.Users!.AddAsync(user);

                _ = await _context.SaveChangesAsync();

                return Ok("A new user has been created");
            }
            else
            {
                return BadRequest("User creation error");
            }
        }

        // DELETE api/user/{id}
        [HttpDelete("{id}", Name = nameof(DeleteUser))]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            // Retrieve the user with the specified id from the database
            User? user = _context.Users!.FirstOrDefault(u => u.Id == id);

            if (user is not null)
            {
                // Remove the user from the database and save changes
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