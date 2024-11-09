namespace StoryPromptMVC.Models.Story
{
	public class CreateStoryVM
	{
		public string storyContent { get; set; }
		public DateTime storyDateCreated { get; set; }
		public int promptId { get; set; }
		public string userId { get; set; }
	}
}
