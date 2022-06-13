using RecipeService.Entities;

namespace RecipeService.Repositories.Contracts;

public interface IRecipeRepository
{
    Task<Recipe> GetRecipe(Guid id, CancellationToken cancellationToken);
}