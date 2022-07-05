namespace RecipeService.Contracts;

public record CreateRecipeRequest(string Title, string Description, List<IngredientRequest> Ingredients);

public record IngredientRequest(string Name); 