using RecipeService.Entities;

namespace RecipeService;

public record CreateRecipeRequest(string Title, List<string> Ingredients);

public class IngredientRequest
{
    public string Name { get; set; }
}