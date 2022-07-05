namespace RecipeService.Features;

public class GetRecipeResult
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    
    public IEnumerable<IngredientResult> Ingredients { get; set; }
}

public class IngredientResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}