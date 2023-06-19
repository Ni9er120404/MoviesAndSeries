using Microsoft.EntityFrameworkCore;
using MoviesAndSeries.Server.API.Data;

namespace MoviesAndSeries.Server.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args); // Create a new instance of WebApplicationBuilder.

			string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Get the connection string from the configuration.

			_ = builder.Services.AddDbContext<Context>(options => options.UseNpgsql(connectionString)); // Add the DbContext to the services with the specified connection string.
			_ = builder.Services.AddControllers(); // Add controllers to the services.
			_ = builder.Services.AddEndpointsApiExplorer(); // Add API Explorer to the services.
			_ = builder.Services.AddSwaggerGen(); // Add Swagger generator to the services.

			using WebApplication app = builder.Build(); // Build the WebApplication instance.

			if (app.Environment.IsDevelopment())
			{
				_ = app.UseSwagger(); // Enable Swagger middleware for API documentation.
				_ = app.UseSwaggerUI(); // Enable Swagger UI for visualizing the API documentation.
			}

			_ = app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS.

			_ = app.UseAuthorization(); // Enable authorization middleware.

			_ = app.MapControllers(); // Map the controllers.

			app.Run(); // Start the application.
		}
	}
}