using MoviesAndSeries.Server.API.Models;
using System.Text.RegularExpressions;

namespace MoviesAndSeries.Server.API.Parser
{
	/// <summary>
	/// Parser class for extracting series information from a URL.
	/// </summary>
	public class SeriesParser
	{
		private readonly string _url;

		/// <summary>
		/// Initializes a new instance of the SeriesParser class with the specified URL.
		/// </summary>
		/// <param name="url">The URL to parse.</param>
		public SeriesParser(string url)
		{
			_url = url;
		}

		/// <summary>
		/// Parses the series information from the provided URL.
		/// </summary>
		/// <returns>A list of parsed series.</returns>
		public async Task<List<Series>> Parse()
		{
			List<Series> result = new(); // Create a new list to store the parsed series.

			foreach (KeyValuePair<Series, string> series in await ParsingInformationAboutAllSeries())
			{
				result.Add(series.Key); // Add each parsed series to the result list.
			}

			return result; // Return the list of parsed series.
		}

		private static async Task<string> ParsingOneSeries(string url)
		{
			string html = await GetHtml(url); // Retrieve the HTML content of the specified URL.
			string ratingPattern = @"<div class=""rate-element long""[^>]+>([\d.]+)</div>"; // Define the pattern to extract the rating.
			string imagePattern = @"<img itemprop=""image"" src=""([^""]+)"" alt=""[^""]+"">"; // Define the pattern to extract the image URL.

			_ = Regex.Match(html, ratingPattern); // Perform a regex match to find the rating element.
			Match imageMatch = Regex.Match(html, imagePattern); // Perform a regex match to find the image element.

			string result = string.Empty;

			if (imageMatch.Success)
			{
				result = imageMatch.Groups[1].Value; // Extract the URL of the series image.
			}

			return result; // Return the URL of the series image.
		}

		private async Task<Dictionary<Series, string>> ParsingInformationAboutAllSeries()
		{
			string html = await GetHtml(_url); // Retrieve the HTML content of the specified URL.
			string pattern = @"<li class=""literal__item not-loaded"" data-kp=""([\d.]+)"" data-imdb=""([\d.]+)"" data-start-year=""(\d{4})"" data-end-year=""(\d{4})"" data-genres=""([^""]+)"" data-countries=""([^""]+)"" data-status=""([^""]+)"" data-id=""(\d+)"">\s*<a href=""([^""]+)"">([^<]+)</a>"; // Define the pattern to extract series information.

			MatchCollection matches = Regex.Matches(html, pattern); // Perform a regex match to find all series matches.

			Dictionary<Series, string> seriesList = new(); // Create a new dictionary to store series information.

			foreach (Match match in matches.Cast<Match>().Take(100))
			{
				string imdb = match.Groups[2].Value; // Extract the IMDb rating of the series.
				string startYear = match.Groups[3].Value; // Extract the start year of the series.
				string endYear = match.Groups[4].Value; // Extract the end year of the series.
				string id = $"{_url}{match.Groups[9].Value}"; // Generate the complete URL for the series.
				string title = match.Groups[10].Value; // Extract the title of the series.
				string poster = await ParsingOneSeries(id); // Retrieve the URL of the series image.

				Series series = new()
				{
					PosterId = id,
					Rating = double.TryParse(imdb, out double rating) ? rating : null, // Parse the IMDb rating and assign it to the series object.
					StartDate = ushort.TryParse(startYear, out ushort startDate) ? startDate : null, // Parse the start year and assign it to the series object.
					EndDate = ushort.TryParse(endYear, out ushort endDate) ? endDate : null, // Parse the end year and assign it to the series object.
					Name = title,
					Image = poster
				};

				seriesList.Add(series, id); // Add the series object to the dictionary.
			}

			return seriesList; // Return the dictionary containing the parsed series information.
		}

		private static async Task<string> GetHtml(string url)
		{
			using HttpClient client = new(); // Create a new HttpClient.

			using HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false); // Send a GET request to the specified URL and retrieve the response.
			string html = string.Empty;

			try
			{
				_ = response.EnsureSuccessStatusCode(); // Ensure that the response has a successful status code.
				html = await response.Content.ReadAsStringAsync().ConfigureAwait(false); // Read the HTML content from the response.
			}
			catch { }

			return html; // Return the HTML content of the page.
		}
	}
}