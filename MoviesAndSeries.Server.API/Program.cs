using Microsoft.EntityFrameworkCore;
using MoviesAndSeries.Server.API.Data;

namespace MoviesAndSeries.Server.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

			_ = builder.Services.AddDbContext<Context>(options => options.UseNpgsql(connectionString));
			_ = builder.Services.AddControllers();
			_ = builder.Services.AddEndpointsApiExplorer();
			_ = builder.Services.AddSwaggerGen();

			using WebApplication app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				_ = app.UseSwagger();
				_ = app.UseSwaggerUI();
			}

			_ = app.UseHttpsRedirection();

			_ = app.UseAuthorization();

			_ = app.MapControllers();

			app.Run();
		}
	}
}