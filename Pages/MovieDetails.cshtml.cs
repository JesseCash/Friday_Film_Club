using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

// had to use some AI for this one as it was a bit too complex and i have never done this before
namespace Friday_Film_Club.Pages
{
    public class MovieDetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MovieDetailsModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public MovieDetails Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var apiKey = _configuration["OMDbApiKey"];
            var url = $"https://www.omdbapi.com/?i={id}&apikey={apiKey}";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync(url);
            Movie = JsonSerializer.Deserialize<MovieDetails>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (Movie == null || Movie.Response == "False")
            {
                return NotFound();
            }

            return Page();
        }
    }

    public class MovieDetails
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string BoxOffice { get; set; }
        public string Poster { get; set; }
        public string Response { get; set; }
    }
}
