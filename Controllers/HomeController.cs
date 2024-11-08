using Microsoft.AspNetCore.Mvc;
using StoryPromptMVC.Models;
using System.Diagnostics;

namespace StoryPromptMVC.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
