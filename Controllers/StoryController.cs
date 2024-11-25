using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoryPromptMVC.Models.Prompt;
using StoryPromptMVC.Models.PromptStory;
using StoryPromptMVC.Models.Story;
using System.Net.Http.Headers;
using System.Text;

namespace StoryPromptMVC.Controllers
{
    public class StoryController : Controller
    {
        private readonly string baseAdress = "http://localhost:5173/api/Story";
        private readonly HttpClient _client;
        public StoryController()
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AdminStoryHandler()
        {
            var response = await _client.GetAsync(baseAdress);
            var content = await response.Content.ReadAsStringAsync();
            var stories = JsonConvert.DeserializeObject<List<StoryVM>>(content);

            return View(stories);
        }

        public async Task<IActionResult> PromptStories(int Id)
        {
            //Get the prompt
            var promptResponse = await _client.GetAsync($"http://localhost:5173/api/Prompt/{Id}");
            var promptJson = await promptResponse.Content.ReadAsStringAsync();
            var prompt = JsonConvert.DeserializeObject<PromptByIdVM>(promptJson);

            //Get the Stories for prompt
            var storyResponse = await _client.GetAsync($"{baseAdress}/all/{Id}");
            var storyJson = await storyResponse.Content.ReadAsStringAsync();
            var storiesForPrompt = JsonConvert.DeserializeObject<IEnumerable<StoryByPromptVM>>(storyJson);

            var promptAndStories = new PromptWithStoriesVM
            {
                Prompt = prompt,
                Stories = storiesForPrompt
            };

            return View(promptAndStories);
        }

        public async Task<IActionResult> CreateStory(int promptId)
        {
			

			var response = await _client.GetAsync($"http://localhost:5173/api/Prompt/{promptId}");
            var json = await response.Content.ReadAsStringAsync();
            var prompt = JsonConvert.DeserializeObject<PromptVM>(json);

            ViewBag.prompt = prompt;
            ViewBag.propmtId = promptId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(CreateStoryVM story)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			var token = HttpContext.Session.GetString("JwtToken"); //Get the token from Session and get the userID from backend

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var json = JsonConvert.SerializeObject(story);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(baseAdress, content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStory(int storyId)
        {
            if (storyId == null)
            {
                return BadRequest(ModelState);
            }

            var response = await _client.DeleteAsync($"{baseAdress}/{storyId}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(ModelState);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditStory(int storyId)
        {
            var response = await _client.GetAsync($"{baseAdress}/{storyId}");
            var json = await response.Content.ReadAsStringAsync();
            var story = JsonConvert.DeserializeObject<PromptVM>(json);

            return View(story);
        }

        [HttpPost]
        public async Task<IActionResult> EditStory(StoryVM storyToEdit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var json = JsonConvert.SerializeObject(storyToEdit);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{baseAdress}/{storyToEdit.id}", content);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");
        }
    }
}
