using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryPromptMVC.Models.Prompt
{
    public class CreatePromptVM
    {
        //public int id { get; set; }
        public string promptContent { get; set; }
        public DateTime promptDateCreated { get; set; }
    }
}