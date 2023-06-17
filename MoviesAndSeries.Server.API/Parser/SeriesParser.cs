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

			foreach (KeyValuePair<Series, string> series in await ParsingOneSeries())
			{
				result.Add(series.Key);
			}

			return result;
		}

		private async Task<Dictionary<Series, string>> ParsingOneSeries()
		{
			Dictionary<Series, string> pairs = new();

			foreach (KeyValuePair<Series, string> item in await ParsingInformationAboutAllSeries())
			{
				string html = await GetHtml(item.Value);
				string pattern = @"<img itemprop=""image"" src=""([^""]+)"" alt=""[^""]+"">";

				Match match = Regex.Match(html, pattern);

				if (match.Success)
				{
					string imageUrl = match.Groups[1].Value;
					item.Key.Image = _url + imageUrl;
					pairs.Add(item.Key, imageUrl);
				}
			}

			return pairs;
		}

		private async Task<Dictionary<Series, string>> ParsingInformationAboutAllSeries()
		{
			string html = await GetHtml(_url);
			string pattern = @"</li> <li class=""literal__item not-loaded"" data-kp=""([\d.]+)"" data-imdb=""([\d.]+)"" data-start-year=""(\d{4})"" data-end-year=""(\d{4})"" data-genres=""([^""]+)"" data-countries=""([^""]+)"" data-status=""([^""]+)"" data-id=""(\d+)"">\s*<a href=""([^""]+)"">([^<]+)</a>";

			MatchCollection matches = Regex.Matches(html, pattern);

			Dictionary<Series, string> seriesList = new();

			for (int i = 0; i < matches.Count; i++)
			{
				Match match = matches[i];

				string imdb = match.Groups[2].Value;
				string startYear = match.Groups[3].Value;
				string endYear = match.Groups[4].Value;
				string id = match.Groups[9].Value;
				string title = match.Groups[10].Value;

				Series series = new()
				{
					Name = title,
					Rating = (ushort?)(ushort.TryParse(imdb, out ushort rating) ? rating : 0),
					StartDate = (ushort?)(ushort.TryParse(startYear, out ushort startDate) ? startDate : 0),
					EndDate = (ushort?)(ushort.TryParse(endYear, out ushort endDate) ? endDate : 0),
				};

				seriesList.Add(series, id);
			}

			return seriesList;
		}

		private static async Task<string> GetHtml(string url)
		{
			string html = string.Empty;
			try
			{
				using HttpClient client = new();

				html = await client.GetStringAsync(url);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Ошибка при получении HTML: " + ex.Message);
			}

			return html;
		}
	}
}