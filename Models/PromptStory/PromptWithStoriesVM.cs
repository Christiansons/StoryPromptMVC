using StoryPromptMVC.Models.Prompt;
using StoryPromptMVC.Models.Story;

namespace StoryPromptMVC.Models.PromptStory
{
    public class PromptWithStoriesVM
    {
        public PromptByIdVM Prompt { get; set; }
        public IEnumerable<StoryByPromptVM> Stories { get; set; }
    }
}
