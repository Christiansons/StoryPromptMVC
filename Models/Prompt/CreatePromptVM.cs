using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryPromptMVC.Models.Prompt
{
    public class CreatePromptVM
    {
        //public int id { get; set; } Get from cookie
        public string UserId { get; set; }
        public string PromptContent { get; set; }
        
    }
}