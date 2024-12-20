﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoryPromptMVC.Models.User;
using System.Text;

namespace StoryPromptMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly string baseAdress = "https://promptlyapi.azurewebsites.net/api/user";
        private readonly HttpClient _client;
        public UserController()
        {
            _client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AdminUserHandler()
        {
            var response = await _client.GetAsync(baseAdress);
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<UserVM>>(content);

            return View(users);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserVM user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("model e fel");
            }
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(baseAdress, content);
            
            if(!response.IsSuccessStatusCode)
            {
                return BadRequest("fel med api");
            }

            return RedirectToAction("AdminUserHandler");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if(userId == null)
            {
                return BadRequest(ModelState);
            }

            var response = await _client.DeleteAsync($"{baseAdress}/{userId}");
            if(!response.IsSuccessStatusCode)
            {
                return BadRequest(ModelState);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditUser(string userId)
        {
            var response = await _client.GetAsync($"{baseAdress}/{userId}");
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserVM>(json);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserVM userToEdit)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var json = JsonConvert.SerializeObject(userToEdit);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{baseAdress}/{userToEdit.id}", content);
            if(!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");
        }

    }
}
