using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe_Sharing_Platform.ViewModels
{
    public class AddRecipeViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        [Required]
        public string Ingredients { get; set; }
        public string Steps { get; set; }
    }
}
