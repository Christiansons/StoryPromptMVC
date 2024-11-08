using Microsoft.AspNetCore.Mvc;

namespace StoryPromptMVC.Controllers
{
    public class PromptReactionController : Controller
    {
        private readonly string baseAdress = "http://localhost:5173/api/PromptReaction";
        private readonly HttpClient _client;
        public PromptReactionController()
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
