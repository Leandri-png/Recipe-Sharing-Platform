using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe_Sharing_Platform.Models
{
    public class Comment
    {
        public string CommentId { get; set; }
        public string Content { get; set; }
        public string RecipeId { get; set; }
        public string UserEmail { get; set; }

    }
}
