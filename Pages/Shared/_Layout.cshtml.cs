using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Friday_Film_Club.Pages.Shared
{
    public class _LayoutModel : PageModel
    {
        private readonly string _connectionString;

        public _LayoutModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostSignUpAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO users (username, password) VALUES (@username, @password)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", Account.Username);
            cmd.Parameters.AddWithValue("@password", Account.Password); // Hash this in real apps!

            int result = await cmd.ExecuteNonQueryAsync();
            if (result > 0)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT * FROM users WHERE username = @username AND password = @password";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", Username);
            cmd.Parameters.AddWithValue("@password", Password);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return RedirectToPage("/Index"); // User authenticated
            }

            return Page(); // Login failed
        }
    }

    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
