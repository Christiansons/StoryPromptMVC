using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoryPromptMVC.Models.User;
using System.Text;

namespace StoryPromptMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly string baseAdress = "http://localhost:5173/api/user";
        private readonly HttpClient _client;
        public UserController(HttpClient httpClient)
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserVM user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(baseAdress, content);
            
            if(!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("CreateUser");
        }
    }
}
