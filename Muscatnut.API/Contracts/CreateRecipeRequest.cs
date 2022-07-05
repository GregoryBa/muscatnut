namespace RecipeService.Contracts;

public record CreateRecipeRequest(int Id, string Title, string Description, List<IngredientRequest> Ingredients);

public record IngredientRequest(string Name); 