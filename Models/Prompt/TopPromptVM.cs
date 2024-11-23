using StoryPromptMVC.Models.User;

namespace StoryPromptMVC.Models.Prompt
{
    public class TopPromptVM
    {
        public int Id { get; set; }
        public string PromptContent { get; set; }
        public DateTime PromptDateCreated { get; set; }
        public UserPromptVM user { get; set; }
        public int ReactionCount { get; set; }
    }
}
