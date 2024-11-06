namespace StoryPromptMVC.Models.Story
{
    public class StoryVM
    {
        public int id { get; set; }
        public string storyContent { get; set; }
        public DateTime storyDateCreated { get; set; }
        public int promptId { get; set; }
        public string userId { get; set; }
    }
}
