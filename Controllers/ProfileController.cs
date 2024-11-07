using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoryPromptMVC.Models.Profile;

namespace StoryPromptMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly string baseAdress = "http://localhost:5173/api/Profile";
        private readonly HttpClient _client;
        public ProfileController(HttpClient httpClient)
        {
            _client = new HttpClient();
        }

        public IActionResult Index(int userId)
        {

            return View();
        }

        public async Task<IActionResult> AdminProfileHandler()
        {
            var response = await _client.GetAsync(baseAdress);
            var json = await response.Content.ReadAsStringAsync();
            var profiles = JsonConvert.DeserializeObject<List<ProfileVM>>(json);

            return View(profiles);
        }

        public IActionResult CreateProfile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(ProfileVM profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var json = JsonConvert.SerializeObject(profile);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(baseAdress, content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfile(int profileId)
        {
            if (profileId == null)
            {
                return BadRequest(ModelState);
            }

            var response = await _client.DeleteAsync($"{baseAdress}/{profileId}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(ModelState);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditProfile(int profileId)
        {
            var response = await _client.GetAsync($"{baseAdress}/{profileId}");
            var json = await response.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<ProfileVM>(json);

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileVM profileToEdit)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var json = JsonConvert.SerializeObject(profileToEdit);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{baseAdress}/{profileToEdit.Id}", content);
            if(!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");
        }

    }
}
