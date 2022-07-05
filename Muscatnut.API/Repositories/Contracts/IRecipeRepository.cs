using RecipeService.Models;

namespace RecipeService.Repositories.Contracts;

public interface IRecipeRepository
{
    Task<RecipeEntity> GetRecipe(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<RecipeEntity>> GetAllRecipes(CancellationToken cancellationToken);
}