using Microsoft.EntityFrameworkCore;
using RecipeService.Entities;
using RecipeService.Infrastructure;

namespace RecipeService.Services;

public class RecipeService : IRecipeService
{
    private readonly ServiceContext _context;

    public RecipeService(ServiceContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(Recipe recipe)
    {
        /*var existingRecipe = await GetById(recipe.Id);
        if (existingRecipe is not null)
        {
            return false;
        }*/
        
        var newRecipe = new Recipe()
        {
            Title = recipe.Title,
            Description = recipe.Description,
            Ingredients = recipe.Ingredients
        };

        await _context.Recipes.AddAsync(newRecipe);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<Recipe?> GetById(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Recipe>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Recipe>> SearchByTitleAsync(string searchTerm)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Recipe recipe)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid Id)
    {
        throw new NotImplementedException();
    }
}