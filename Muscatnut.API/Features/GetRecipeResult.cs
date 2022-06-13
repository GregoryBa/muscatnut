namespace RecipeService.Features;

public class GetRecipeResult
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    
    public List<IngredientResult> Ingredients { get; set; }
}

public class IngredientResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbohydrates { get; set; }
    public double Fats { get; set; }
}