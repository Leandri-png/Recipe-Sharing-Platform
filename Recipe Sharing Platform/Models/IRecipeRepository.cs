using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe_Sharing_Platform.Models
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetAllRecipes();
        Recipe GetRecipe(string id);
        Recipe AddRecipe(Recipe recipe);
        Recipe UpdateRecipe(Recipe recipe);
        Recipe DeleteRecipe(Guid id);

    }
}
