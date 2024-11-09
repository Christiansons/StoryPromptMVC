using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace StoryPromptMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly string baseAdress = "http://localhost:5173/api/Profile";
        private readonly HttpClient _client;
        public ProfileController()
        {
            _client = new HttpClient();
			_client.DefaultRequestHeaders.Accept.Clear();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

        public async Task<IActionResult> Index(string? userId = null)
        {
            var token = HttpContext.Session.GetString("JwtToken"); //Get the token from Session

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{baseAdress}/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response);
            }

            var profile = await response.Content.ReadAsStringAsync();
            return View(profile);
        }
    }
}
