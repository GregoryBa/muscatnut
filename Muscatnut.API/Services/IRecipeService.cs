using RecipeService.Contracts;
using RecipeService.Models;

namespace RecipeService.Services;

public interface IRecipeService
{
    public Task<string> CreateAsync(CreateRecipeRequest recipeRequest);

    public Task<RecipeEntity?> GetByHashedId(string hashedId);

    public Task<IEnumerable<RecipeResult>> GetAllAsync();

    public Task<IEnumerable<RecipeResult>> SearchByTitleAsync(string searchTerm);

    public Task<bool> UpdateAsync(CreateRecipeRequest recipeRequest, string hashedId);

    public Task<bool> DeleteAsync(string hashedId);
}