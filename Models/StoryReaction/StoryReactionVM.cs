namespace StoryPromptMVC.Models.StoryReaction
{
    public class StoryReactionVM
    {
        public int Id { get; set; }
        public string Reaction { get; set; }
        public int StoryId { get; set; }
        public string? UserId { get; set; }

    }
}
