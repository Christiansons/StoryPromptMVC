using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryPromptMVC.Models.Prompt
{
    public class UpdatePromptVM
    {
        public int Id { get; set; }
        public string PromptContent { get; set; }
    }
}