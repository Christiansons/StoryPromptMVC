namespace StoryPromptMVC.Models.Profile
{
	public class ProfileByIdVM
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public string Picture { get; set; }
		public DateOnly ProfileCreated { get; set; }
		public string UserId { get; set; }
		public bool IsOwnProfile { get; set; }
	}
}
