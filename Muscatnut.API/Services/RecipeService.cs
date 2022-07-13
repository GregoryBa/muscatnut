using HashidsNet;
using Microsoft.EntityFrameworkCore;
using RecipeService.Contracts;
using RecipeService.Features;
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

    public async Task<string> CreateAsync(CreateRecipeRequest recipeRequest)
    {
        var newRecipe = new RecipeEntity()
        {
            Title = recipeRequest.Title,
            Description = recipeRequest.Description,
            Ingredients = recipeRequest.Ingredients.Select(x => new IngredientEntity()
            {
                Name = x.Name
            }).ToList()
        };

        await _context.Recipes.AddAsync(newRecipe);
        await _context.SaveChangesAsync();
        return _hashids.Encode(newRecipe.Id);
    }

    public async Task<RecipeEntity?> GetByHashedId(string hashedId) => 
        await _context.Recipes.FirstOrDefaultAsync(x => x.Id == _hashids.DecodeSingle(hashedId));


    public async Task<IEnumerable<RecipeResult>> GetAllAsync()
    {
        var allRecipes = await _context.Recipes.Include(x=> x.Ingredients).ToListAsync();
        var recipeResult = allRecipes.Select(x => new RecipeResult()
        {
            HashedId = _hashids.Encode(x.Id),
            Description = x.Description,
            Title = x.Title,
            Ingredient =  x.Ingredients.Select(x => new IngredientResult() {Name = x.Name}).ToList()
        });
        return recipeResult;
    }

    public async Task<IEnumerable<RecipeResult>> SearchByTitleAsync(string searchTerm)
    {
        var matchedRecipes = await _context.Recipes.Include(x=> x.Ingredients)
            .Where(x => 
            x.Title.Contains(@searchTerm)).ToListAsync();
        var recipeResult = matchedRecipes.Select(x => new RecipeResult()
        {
            HashedId = _hashids.Encode(x.Id),
            Description = x.Description,
            Title = x.Title,
            Ingredient = x.Ingredients.Select(x => new IngredientResult() {Name = x.Name}).ToList()
        });
        return recipeResult;
    }
    

    public async Task<bool> UpdateAsync(CreateRecipeRequest recipeRequest, string hashedId)
    {
        var recipe = await _context.Recipes.Include(x=> x.Ingredients)
            .FirstOrDefaultAsync(x => x.Id == _hashids.DecodeSingle(hashedId));
        if (recipe == null) return false;
        
        recipe.Description = recipeRequest.Description;
        recipe.Title = recipeRequest.Title;
        recipe.Ingredients = recipeRequest.Ingredients.Select(x => new IngredientEntity() { Name = x.Name }).ToList();
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string hashedId)
    {
        var recipe = await _context.Recipes.Include(x=> x.Ingredients)
            .FirstOrDefaultAsync(x => x.Id == _hashids.DecodeSingle(hashedId));
        if (recipe == null) return false;

        _context.Remove(recipe);
        await _context.SaveChangesAsync();
        return true;
    }
}