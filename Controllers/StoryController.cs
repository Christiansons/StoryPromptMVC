using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoryPromptMVC.Models;
using StoryPromptMVC.Models.Prompt;
using StoryPromptMVC.Models.PromptStory;
using StoryPromptMVC.Models.Story;
using System.Net.Http.Headers;
using System.Text;

namespace StoryPromptMVC.Controllers
{
    public class StoryController : Controller
    {
        private readonly string baseAdress = "https://promptlyapi.azurewebsites.net/api/Story";
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;

        
        public StoryController(IHttpClientFactory httpClientFactory)
        {
            _client = new HttpClient();
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Story/Create/{promptId}
        [HttpGet]
        public IActionResult CreateStory(int Id)

        {
            var userId = User.FindFirst("sub")?.Value;
            var model = new CreateStoryViewModel
            {
                PromptId = Id,
                UserId = userId
            };

            return View(model);
        }

        // POST: /Story/CreateStory
        [HttpPost]
        public async Task<IActionResult> CreateStory(CreateStoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }

            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Populate missing fields
            model.UserId = userId;

            var client = _httpClientFactory.CreateClient("StoryPromptAPI");
            var response = await client.PostAsJsonAsync("/api/Story", model);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to create story.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Story created successfully!";
            return RedirectToAction("top", "prompt");
        }
        [HttpGet]
        public async Task<IActionResult> StoriesForPrompt(int promptId)
        {
            var client = _httpClientFactory.CreateClient("StoryPromptAPI");
            var response = await client.GetAsync($"/api/Story/prompt/{promptId}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to fetch stories for the prompt.";
                return RedirectToAction("Index", "Home");
            }

            var stories = await response.Content.ReadFromJsonAsync<IEnumerable<StoryViewModel>>();
            // Fetch usernames for each userId
            foreach (var story in stories)

            {
                if (!string.IsNullOrEmpty(story.UserId))
                {
                    var userResponse = await client.GetAsync($"/api/User/{story.UserId}");
                    if (userResponse.IsSuccessStatusCode)
                    {
                        var user = await userResponse.Content.ReadFromJsonAsync<UserViewModel>();
                        story.UserName = user?.UserName ?? "Unknown";
                    }
                    else
                    {
                        story.UserName = "Unknown";
                    }
                }
            }
            return View(stories);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("StoryPromptAPI");
            var response = await client.GetAsync($"/api/Story/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to fetch story for editing.";
                return RedirectToAction("Details", "Prompt", new { id = id });
            }

            var story = await response.Content.ReadFromJsonAsync<StoryViewModel>();
            return View(story);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }

            // Ensure UserId and UserName are populated
            if (string.IsNullOrEmpty(model.UserId))
            {
                model.UserId = User.FindFirst("sub")?.Value;
            }

            if (string.IsNullOrEmpty(model.UserName))
            {
                model.UserName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "Unknown";
            }

            var client = _httpClientFactory.CreateClient("StoryPromptAPI");
            var response = await client.PutAsJsonAsync($"/api/Story/{model.Id}", model);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to update the story.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Story updated successfully!";
            return RedirectToAction("Details", "Prompt", new { id = model.PromptId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int storyId, int promptId)
        {
            var client = _httpClientFactory.CreateClient("StoryPromptAPI");
            var response = await client.DeleteAsync($"/api/Story/{storyId}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to delete the story.";
                return RedirectToAction("Details", "Prompt", new { id = promptId }); // Redirect back to the correct prompt
            }

            TempData["SuccessMessage"] = "Story deleted successfully!";
            return RedirectToAction("Details", "Prompt", new { id = promptId }); // Redirect to the prompt details
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id, int PromptId)
        {
            var client = _httpClientFactory.CreateClient("StoryPromptAPI");
            var response = await client.DeleteAsync($"/api/Story/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to delete the story.";
                return RedirectToAction("Details", "Prompt", new { id = id });
            }

            TempData["SuccessMessage"] = "Story deleted successfully!";
            return RedirectToAction("Details", "Prompt", new { id = PromptId });
        }

        public async Task<IActionResult> PromptStories(int Id)
        {
            //Get the prompt
            var promptResponse = await _client.GetAsync($"https://promptlyapi.azurewebsites.net/api/Prompt/{Id}");
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

       
    }
}
