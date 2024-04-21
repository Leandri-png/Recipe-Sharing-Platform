using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe_Sharing_Platform.Models;
using Recipe_Sharing_Platform.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe_Sharing_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRecipeRepository recipeRepository;

        public IHostingEnvironment HostingEnv;
        private readonly AppDbContext context;

        public HomeController(IRecipeRepository recipeRepository, IHostingEnvironment hostingEnv,
                               AppDbContext context)
        {
            this.recipeRepository = recipeRepository;
            HostingEnv = hostingEnv;
            this.context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Recipe> recipe = recipeRepository.GetAllRecipes();

            return View(recipe);
        }

        [HttpGet]
        public IActionResult AddRecipe()
        {
            return View();
        }

        private string ProcessUploadedFile(AddRecipeViewModel model)
        {
            string uniqueFileName = null;
            if (model.Image != null)
            {

                string uploadsFolder = Path.Combine(HostingEnv.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }


            }

            return uniqueFileName;
        }


        [HttpPost]
        public IActionResult AddRecipe(AddRecipeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                var recipe = new Recipe
                {
                    Title = model.Title,
                    Description = model.Description,
                    Ingredients = model.Ingredients,
                    Steps = model.Steps,
                    Photo = uniqueFileName
                };

                recipeRepository.AddRecipe(recipe);

                return RedirectToAction("Index");

            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(string recipeId)
        {
            var recipe = recipeRepository.GetRecipe(recipeId);

            return View(recipe);
        }

        [HttpPost]
        public IActionResult AddComment(string userEmail, string commentContent, string recipeId)
        {
            var comm = new Comment
            {
                RecipeId = recipeId,
                UserEmail = userEmail,
                Content = commentContent
            };

            context.Comments.Add(comm);

            context.SaveChanges();

            return RedirectToAction("Details", new { recipeId });
        }

		[HttpGet]
		public IActionResult EditRecipe(string id)
		{
			var recipe = recipeRepository.GetRecipe(id);
			if (recipe == null)
			{
				return NotFound();
			}

			return View(recipe);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditRecipe(string id, Recipe updatedRecipe)
		{
			if (id != updatedRecipe.RecipeId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					var existingRecipe = recipeRepository.GetRecipe(id);
					if (existingRecipe == null)
					{
						return NotFound();
					}

					existingRecipe.Title = updatedRecipe.Title;
					existingRecipe.Description = updatedRecipe.Description;
					existingRecipe.Ingredients = updatedRecipe.Ingredients;
					existingRecipe.Steps = updatedRecipe.Steps;

					recipeRepository.UpdateRecipe(existingRecipe);

					return RedirectToAction("Index");
				}
				catch (Exception)
				{
					// Handle exception
					return View(updatedRecipe);
				}
			}

			return View(updatedRecipe);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRecipe(string id)
        {
            if (!Guid.TryParse(id, out Guid recipeId))
            {
                return BadRequest("Invalid recipe ID format");
            }

            var recipe = recipeRepository.GetRecipe(id);
            if (recipe == null)
            {
                return NotFound();
            }

            recipeRepository.DeleteRecipe(recipeId);
            return RedirectToAction("Index");
        }

    }
}
