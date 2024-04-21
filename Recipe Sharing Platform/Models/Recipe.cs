using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe_Sharing_Platform.Models
{
    public class Recipe
    {
        public Recipe()
        {
			RecipeId = Guid.NewGuid().ToString(); // Generate a unique identifier
			Comments = new List<Comment>();
        }

        [Required]
        public string RecipeId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Photo{ get; set; }
        [Required]
        public string Ingredients { get; set; }
        public string Steps { get; set; }

        public List<Comment> Comments { get; set; }
        
    }
}
