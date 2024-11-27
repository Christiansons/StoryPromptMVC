using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoryPromptMVC.Models.Prompt;
using StoryPromptMVC.Models.User;
using System.Text;

namespace StoryPromptMVC.Controllers
{
    public class PromptController : Controller
    {
        private readonly string baseAdress = "https://promptlyapi.azurewebsites.net/api/Prompt";
        private readonly HttpClient _client;
        public PromptController()
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AdminPromptHandler()
        {
            var response = await _client.GetAsync(baseAdress);
            var content = await response.Content.ReadAsStringAsync();
            var prompts = JsonConvert.DeserializeObject<List<PromptVM>>(content);

            return View(prompts);
        }

        public IActionResult CreatePrompt()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrompt(CreatePromptVM prompt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var json = JsonConvert.SerializeObject(prompt);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(baseAdress, content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Prompt");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePrompt(int promptId)
        {
            if (promptId == null)
            {
                return BadRequest(ModelState);
            }

            var response = await _client.DeleteAsync($"{baseAdress}/{promptId}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(ModelState);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditPrompt(int promptId)
        {
            var response = await _client.GetAsync($"{baseAdress}/{promptId}");
            var json = await response.Content.ReadAsStringAsync();
            var prompt = JsonConvert.DeserializeObject<PromptVM>(json);

            return View(prompt);
        }

        [HttpPost]
        public async Task<IActionResult> EditPrompt(PromptVM promptToEdit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var json = JsonConvert.SerializeObject(promptToEdit);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{baseAdress}/{promptToEdit.id}", content);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Top()
        {
            var resposne = await _client.GetAsync($"{baseAdress}/top");
            var json = await resposne.Content.ReadAsStringAsync();
            var topPrompts = JsonConvert.DeserializeObject<IEnumerable<TopPromptVM>>(json);
            return View(topPrompts);
        }

        public async Task<IActionResult> New()
        {
            var resposne = await _client.GetAsync($"{baseAdress}/new");
            var json = await resposne.Content.ReadAsStringAsync();
            var topPrompts = JsonConvert.DeserializeObject<IEnumerable<NewPromptVM>>(json);
            return View(topPrompts);
        }

        [HttpGet]
        public async Task<IActionResult> GetPromptLikes(int id)
        {
            var response = await _client.GetAsync($"{baseAdress}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return Json(new {ReactionCount = 0});
            }
            var json = await response.Content.ReadAsStringAsync();
            var prompt = JsonConvert.DeserializeObject<PromptByIdVM>(json);

            var ReactionCount = prompt.reactionCount;
            return Json(new {ReactionCount});
        }
    }
}

