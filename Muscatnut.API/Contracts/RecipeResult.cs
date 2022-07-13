using RecipeService.Features;

namespace RecipeService.Contracts;

public class RecipeResult
{
    public string HashedId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public List<IngredientResult> Ingredient { get; set; } = new List<IngredientResult>();
}