﻿using StoryPromptMVC.Models.User;

namespace StoryPromptMVC.Models.Prompt
{
    public class PromptVM
    {
        public int id { get; set; }
        public string promptContent { get; set; }
        public DateTime promptDateCreated { get; set; }
        public UserPromptVM user { get; set; }
        public int ReactionCount { get; set; }
        public int StoryCount { get; set; }
    }
}
