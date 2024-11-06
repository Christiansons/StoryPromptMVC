using Microsoft.AspNetCore.Mvc;
using StoryPromptMVC.Models;
using System.Diagnostics;

namespace StoryPromptMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly string baseAdress = "http://localhost:5173/api/";
        private readonly HttpClient _client;
        public HomeController(HttpClient httpClient)
        {
            _client = new HttpClient();
        }

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
