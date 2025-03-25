using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


// had to use some AI for this one as it was a bit too complex and i have never done this before
namespace Friday_Film_Club.Pages
{
    public class SearchResultsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SearchResultsModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public string Query { get; set; }
        public List<Movie> Movies { get; set; } = new();

        public async Task OnGetAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                Movies = new List<Movie>();
                return;
            }

            Query = query;
            var apiKey = _configuration["OMDbApiKey"];
            var url = $"https://www.omdbapi.com/?s={query}&apikey={apiKey}";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync(url);
            var result = JsonSerializer.Deserialize<OMDbSearchResult>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Movies = result?.Search ?? new List<Movie>();
        }
    }

    public class OMDbSearchResult
    {
        public List<Movie> Search { get; set; }
    }

    public class Movie
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Poster { get; set; }
        public string ImdbID { get; set; }
    }
}