using Microsoft.AspNetCore.Mvc;

namespace StoryPromptMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly string baseAdress = "http://localhost:5173/api/Profile";
        private readonly HttpClient _client;
        public ProfileController(HttpClient httpClient)
        {
            _client = new HttpClient();
        }

        public IActionResult Index(int userId)
        {

            return View();
        }
    }
}
