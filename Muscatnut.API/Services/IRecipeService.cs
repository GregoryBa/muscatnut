using RecipeService.Models;

namespace RecipeService.Services;

public interface IRecipeService
{
    public Task<Guid?> CreateAsync(RecipeEntity recipeEntity);

    public Task<RecipeEntity?> GetById(Guid Id);

    public Task<IEnumerable<RecipeEntity>> GetAllAsync();

    public Task<IEnumerable<RecipeEntity>> SearchByTitleAsync(string searchTerm);

    public Task<bool> UpdateAsync(RecipeEntity recipeEntity);

    public Task<bool> DeleteAsync(Guid Id);
}