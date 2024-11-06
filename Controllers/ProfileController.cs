using Microsoft.AspNetCore.Mvc;

namespace StoryPromptMVC.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index(int userId)
        {

            return View();
        }
    }
}
