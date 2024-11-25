using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoryPromptMVC.Models.PromptReaction;
using System.Text;

namespace StoryPromptMVC.Controllers
{
    public class PromptReactionController : Controller
    {
        private readonly string baseAdress = "http://localhost:5173/api/PromptReaction";
        private readonly HttpClient _client;
        public PromptReactionController()
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AdminPromptReactionHandler()
        {
            var response = await _client.GetAsync(baseAdress);
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<PromptReactionVM>>(content);

            return View(users);
        }

        public IActionResult CreateReaction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReaction(PromptReactionVM promptReaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var json = JsonConvert.SerializeObject(promptReaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(baseAdress, content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("AdminPromptReactionHandler");
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

            return RedirectToAction("AdminPromptReactionHandler");
        }

        public async Task<IActionResult> EditReaction(int reactionId)
        {
            var response = await _client.GetAsync($"{baseAdress}/{reactionId}");
            var json = await response.Content.ReadAsStringAsync();
            var promptReaction = JsonConvert.DeserializeObject<PromptReactionVM>(json);

            return View(promptReaction);
        }

        [HttpPost]
        public async Task<IActionResult> EditReaction(PromptReactionVM reactionToEdit)
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

            return RedirectToAction("AdminPromptReactionHandler");
        }

        [HttpPost]
        public async Task<IActionResult> Upvote(int promptId)
        {
			var userId = User.FindFirst("NameIdentifier").Value;
            if(userId == null)
            {
				return View("user", "login");
			}

            var reaction = new AddPromptReactionVM
            {
                PromptId = promptId,
                Reaction = "Upvote",
                UserId = userId
            };

            var json = JsonConvert.SerializeObject(reaction);
            var content = new StringContent(json, Encoding.UTF8 , "application/json");
            var response = await _client.PostAsync($"{baseAdress}", content);


            return RedirectToAction("prompt", "top");
        }

		[HttpPost]
		public async Task<IActionResult> Downvote(int promptId)
		{
			var userId = User.FindFirst("NameIdentifier").Value;
			if (userId == null)
			{
				return View("user", "login");
			}

			var reaction = new AddPromptReactionVM
			{
				PromptId = promptId,
				Reaction = "Downvote",
				UserId = userId
			};

			var json = JsonConvert.SerializeObject(reaction);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync($"{baseAdress}", content);


			return RedirectToAction("prompt", "top");
		}

	}
}
