using MoviesAndSeries.Server.API.Models;
using System.Text.RegularExpressions;

namespace MoviesAndSeries.Server.API.Parser
{
	/// <summary>
	/// Parses series information from a specified URL.
	/// </summary>
	public class SeriesParser
	{
		private readonly string _url;

		private static readonly HttpClient _httpClient = new();

		/// <summary>
		/// Initializes a new instance of the SeriesParser class with the specified URL.
		/// </summary>
		/// <param name="url">The URL to parse series information from.</param>
		public SeriesParser(string url)
		{
			_url = url;
		}

		/// <summary>
		/// Parses the series information from the specified URL.
		/// </summary>
		/// <returns>A list of parsed series.</returns>
		public async Task<List<Series>> Parse()
		{
			string html = await GetHtml(_url).ConfigureAwait(false);

			List<Series> result = await ParsingInformationAboutAllSeries(html);

			return result;
		}

		private static async Task<string> GetHtml(string url)
		{
			HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			}

			return string.Empty;
		}

		private async Task<List<Series>> ParsingInformationAboutAllSeries(string html)
		{
			// Regular expression pattern to match series information
			const string pattern = @"<li class=""literal__item not-loaded"" data-kp=""([\d.]+)"" data-imdb=""([\d.]+)"" data-start-year=""(\d{4})"" data-end-year=""(\d{4})"" data-genres=""([^""]+)"" data-countries=""([^""]+)"" data-status=""([^""]+)"" data-id=""(\d+)"">\s*<a href=""([^""]+)"">([^<]+)</a>";

			MatchCollection matches = Regex.Matches(html, pattern);

			List<Task<Series>> seriesTasks = matches.Cast<Match>()
				.Select(async match =>
				{
					_ = double.TryParse(match.Groups[2].Value, out double imdb);
					_ = ushort.TryParse(match.Groups[3].Value, out ushort startYear);
					_ = ushort.TryParse(match.Groups[4].Value, out ushort endYear);

					string id = $"{_url}{match.Groups[9].Value}";
					string title = match.Groups[10].Value;
					string poster = await ParsingOneSeries(id).ConfigureAwait(false);

					return new Series
					{
						PosterId = id,
						Rating = imdb,
						StartDate = startYear,
						EndDate = endYear,
						Name = title,
						Image = poster
					};
				})
				.ToList();

			Series[] seriesArray = await Task.WhenAll(seriesTasks).ConfigureAwait(false);

			return seriesArray.ToList();
		}

		private static async Task<string> ParsingOneSeries(string url)
		{
			string html = await GetHtml(url).ConfigureAwait(false);
			// Regular expression pattern to match the series image URL
			const string imagePattern = @"<img itemprop=""image"" src=""([^""]+)"" alt=""[^""]+"">";

			Match imageMatch = Regex.Match(html, imagePattern);

			if (imageMatch.Success)
			{
				return imageMatch.Groups[1].Value;
			}

			return string.Empty;
		}
	}
}