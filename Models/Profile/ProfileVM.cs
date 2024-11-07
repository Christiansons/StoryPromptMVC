using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryPromptMVC.Models.Profile
{
    public class ProfileVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public DateOnly ProfileCreated { get; set; }
        public string UserId { get; set; }
    }
}