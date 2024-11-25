using StoryPromptMVC.Models.User;

namespace StoryPromptMVC.Models.Story
{
    public class StoryByPromptVM
    {
        public int Id { get; set; }
        public string StoryContent { get; set; }
        public DateTime StoryDateCreated { get; set; }
        public UserStoryVM user { get; set; }
        public int ReactionCount { get; set; }
    }
}
