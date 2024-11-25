
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoryPromptMVC.Models.Profile;
using StoryPromptMVC.Models.User;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace StoryPromptMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly string _baseAddress = "http://localhost:5173/api/Profile";
        private readonly HttpClient _client;
        public ProfileController()
        {
            _client = new HttpClient();
			_client.DefaultRequestHeaders.Accept.Clear();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

        //private void SetAuthorizationHeader()
        //{
        //    var token = HttpContext.Session.GetString("JwtToken"); // Get token from session
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    }
        //}

        public async Task<IActionResult> Index(string userId = null)
        {
            //SetAuthorizationHeader();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            var response = await _client.GetAsync($"{_baseAddress}/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to retrieve profile.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<ProfileByIdVM>(json);

            return View(profile);
        }
		
		public IActionResult TestView()
		{
			var testProfile = new ProfileVM
			{
				Id = 1,
				Description = "This is a test description",
				Picture = "test-image.png",
				ProfileCreated = DateOnly.FromDateTime(DateTime.Now),
				UserId = "user123"
			};

			return View("Index", testProfile); // Specify the view name if it's not "TestView"
		}

		public async Task<IActionResult> AdminProfileHandler()
        {
            //SetAuthorizationHeader();

            var response = await _client.GetAsync(_baseAddress);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to retrieve profiles.");
            }

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
                return View(profile);
            }

            //SetAuthorizationHeader();

            var json = JsonConvert.SerializeObject(profile);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_baseAddress, content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to create profile.");
                return View(profile);
            }

            return RedirectToAction("AdminProfileHandler");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfile(string userId)
        {
            if (string.IsNullOrEmpty(userId))

            {
                return BadRequest("User ID is required.");
            }
            //SetAuthorizationHeader();

            var response = await _client.DeleteAsync($"{_baseAddress}/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to delete profile.");
            }

            return RedirectToAction("AdminProfileHandler");
        }

        public async Task<IActionResult> EditProfile(string userId)

        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            //SetAuthorizationHeader();

            var response = await _client.GetAsync($"{_baseAddress}/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to retrieve profile for editing.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<ProfileVM>(json);

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileVM profileToEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(profileToEdit);
            }
            //SetAuthorizationHeader();

            var json = JsonConvert.SerializeObject(profileToEdit);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{_baseAddress}/{profileToEdit.UserId}", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to update profile.");
                return View(profileToEdit);
            }

            return RedirectToAction("AdminProfileHandler");
        }

    }
}
