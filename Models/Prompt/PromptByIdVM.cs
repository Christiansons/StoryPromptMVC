using StoryPromptMVC.Models.User;

namespace StoryPromptMVC.Models.Prompt
{
    public class PromptByIdVM
    {
        public int id { get; set; }
        public string promptContent { get; set; }
        public DateTime promptDateCreated { get; set; }
        public UserPromptVM user { get; set; }
        public int reactionCount { get; set; }
        public int storyCount { get; set; }
    }
}
