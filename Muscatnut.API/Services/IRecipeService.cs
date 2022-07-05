using RecipeService.Entities;

namespace RecipeService.Services;

public interface IRecipeService
{
    public Task<bool> CreateAsync(Recipe recipe);

    public Task<Recipe?> GetById(Guid Id);

    public Task<IEnumerable<Recipe>> GetAllAsync();

    public Task<IEnumerable<Recipe>> SearchByTitleAsync(string searchTerm);

    public Task<bool> UpdateAsync(Recipe recipe);

    public Task<bool> DeleteAsync(Guid Id);
}