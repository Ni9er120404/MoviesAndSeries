using MoviesAndSeries.Server.API.Models;
using System.Text.RegularExpressions;

namespace MoviesAndSeries.Server.API.Parser
{
	public class SeriesParser
	{
		private readonly string _url;

		public SeriesParser(string url)
		{
			_url = url;
		}

		public async Task<List<Series>> Parse()
		{
			List<Series> result = new();

			foreach (KeyValuePair<Series, string> series in await ParsingInformationAboutAllSeries())
			{
				result.Add(series.Key);
			}

			return result;
		}

		private static async Task<string> ParsingOneSeries(string url)
		{
			string html = await GetHtml(url);
			string ratingPattern = @"<div class=""rate-element long""[^>]+>([\d.]+)</div>";
			string imagePattern = @"<img itemprop=""image"" src=""([^""]+)"" alt=""[^""]+"">";
			_ = Regex.Match(html, ratingPattern);
			Match imageMatch = Regex.Match(html, imagePattern);

			string result = string.Empty;

			if (imageMatch.Success)
			{
				result = imageMatch.Groups[1].Value;
			}

			return result;
		}

		private async Task<Dictionary<Series, string>> ParsingInformationAboutAllSeries()
		{
			string html = await GetHtml(_url);
			string pattern = @"<li class=""literal__item not-loaded"" data-kp=""([\d.]+)"" data-imdb=""([\d.]+)"" data-start-year=""(\d{4})"" data-end-year=""(\d{4})"" data-genres=""([^""]+)"" data-countries=""([^""]+)"" data-status=""([^""]+)"" data-id=""(\d+)"">\s*<a href=""([^""]+)"">([^<]+)</a>";

			MatchCollection matches = Regex.Matches(html, pattern);

			Dictionary<Series, string> seriesList = new();

			foreach (Match match in matches.Cast<Match>().Take(100))
			{
				string imdb = match.Groups[2].Value;
				string startYear = match.Groups[3].Value;
				string endYear = match.Groups[4].Value;
				string id = $"{_url}{match.Groups[9].Value}";
				string title = match.Groups[10].Value;
				string poster = await ParsingOneSeries(id);

				Series series = new()
				{
					PosterId = id,
					Rating = double.TryParse(imdb, out double rating) ? rating : null,
					StartDate = ushort.TryParse(startYear, out ushort startDate) ? startDate : null,
					EndDate = ushort.TryParse(endYear, out ushort endDate) ? endDate : null,
					Name = title,
					Image = poster
				};

				seriesList.Add(series, id);
			}

			return seriesList;
		}

		public static async Task<string> GetHtml(string url)
		{
			using HttpClient client = new();

			using HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
			string html = string.Empty;
			try
			{
				_ = response.EnsureSuccessStatusCode();
				html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			}
			catch { }

			return html;
		}
	}
}