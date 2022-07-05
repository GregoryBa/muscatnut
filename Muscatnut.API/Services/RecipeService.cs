using Microsoft.EntityFrameworkCore;
using RecipeService.Infrastructure;
using RecipeService.Models;

namespace RecipeService.Services;

public class RecipeService : IRecipeService
{
    private readonly ServiceContext _context;

    public RecipeService(ServiceContext context)
    {
        _context = context;
    }

    public async Task<Guid?> CreateAsync(RecipeEntity recipeEntity)
    {
        /*var existingRecipe = await GetById(recipe.Id);
        if (existingRecipe is not null)
        {
            return false;
        }*/
        
        var newRecipe = new RecipeEntity()
        {
            Id = Guid.NewGuid(),
            Title = recipeEntity.Title,
            Description = recipeEntity.Description,
            Ingredients = recipeEntity.Ingredients
        };

        await _context.Recipes.AddAsync(newRecipe);
        await _context.SaveChangesAsync();
        return newRecipe.Id;
    }

    public Task<RecipeEntity?> GetById(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RecipeEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RecipeEntity>> SearchByTitleAsync(string searchTerm)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(RecipeEntity recipeEntity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid Id)
    {
        throw new NotImplementedException();
    }
}