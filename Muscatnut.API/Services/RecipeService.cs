using HashidsNet;
using Microsoft.EntityFrameworkCore;
using RecipeService.Infrastructure;
using RecipeService.Models;

namespace RecipeService.Services;

public class RecipeService : IRecipeService
{
    private readonly ServiceContext _context;
    private readonly IHashids _hashids;


    public RecipeService(ServiceContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    public async Task<string> CreateAsync(RecipeEntity recipeEntity)
    {
        var newRecipe = new RecipeEntity()
        {
            Title = recipeEntity.Title,
            Description = recipeEntity.Description,
            Ingredients = recipeEntity.Ingredients
        };

        await _context.Recipes.AddAsync(newRecipe);
        await _context.SaveChangesAsync();
        return _hashids.Encode(newRecipe.Id);
    }
    
    public Task<RecipeEntity?> GetById(int Id)
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