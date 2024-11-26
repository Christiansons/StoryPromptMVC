using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoryPromptMVC.Models.StoryReaction;
using System.Text;

namespace StoryPromptMVC.Controllers
{
    public class StoryReactionController : Controller
    {
        private readonly string baseAdress = "https://promptlyapi.azurewebsites.net/api/StoryReaction";
        private readonly HttpClient _client;
        public StoryReactionController()
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AdminStoryReactionHandler()
        {
            var response = await _client.GetAsync(baseAdress);
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<StoryReactionVM>>(content);

            return View(users);
        }

        public IActionResult CreateReaction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReaction(StoryReactionVM story)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var json = JsonConvert.SerializeObject(story);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(baseAdress, content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("AdminStoryReactionHandler");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReaction(int reactionId)
        {
            if (reactionId == null)
            {
                return BadRequest(ModelState);
            }

            var response = await _client.DeleteAsync($"{baseAdress}/{reactionId}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(ModelState);
            }

            return RedirectToAction("AdminStoryReactionHandler");
        }

        public async Task<IActionResult> EditReaction(int reactionId)
        {
            var response = await _client.GetAsync($"{baseAdress}/{reactionId}");
            var json = await response.Content.ReadAsStringAsync();
            var storyReaction = JsonConvert.DeserializeObject<StoryReactionVM>(json);

            return View(storyReaction);
        }

        [HttpPost]
        public async Task<IActionResult> EditReaction(StoryReactionVM reactionToEdit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var json = JsonConvert.SerializeObject(reactionToEdit);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{baseAdress}/{reactionToEdit.Id}", content);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("AdminStoryReactionHandler");
        }

    }
}

