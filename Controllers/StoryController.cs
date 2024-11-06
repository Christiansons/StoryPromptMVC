using Microsoft.AspNetCore.Mvc;

namespace StoryPromptMVC.Controllers
{
    public class StoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
