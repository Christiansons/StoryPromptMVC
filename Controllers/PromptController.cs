using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoryPromptMVC.Models.Prompt;
using StoryPromptMVC.Models.User;
using System.Text;

namespace StoryPromptMVC.Controllers
{
    public class PromptController : Controller
    {
        private readonly string baseAdress = "http://localhost:5173/api/Prompt";
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
        public async Task<IActionResult> CreatePrompt(PromptVM prompt)
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

            return RedirectToAction("Index");
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

    }
}

