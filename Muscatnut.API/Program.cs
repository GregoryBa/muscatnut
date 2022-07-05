using Microsoft.EntityFrameworkCore;
using RecipeService;
using RecipeService.Entities;
using RecipeService.Features;
using RecipeService.Infrastructure;
using RecipeService.Services;

var builder = WebApplication.CreateBuilder(args);
// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ServiceContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnectionString"))
    );
builder.Services.AddSingleton<IRecipeService, RecipeService.Services.RecipeService>();

var app = builder.Build();
// Middleware
app.UseSwagger();
app.UseSwaggerUI();

// Endpoints

app.MapPost("recipe", async (Recipe recipe, IRecipeService recipeService) =>
{
    var created = await recipeService.CreateAsync(recipe);
    if (!created)
    {
        return Results.BadRequest(new
        {
            errorMessage = "A recipe with this title already exists"
        });
        
    }

    return Results.Created($"/recipe/{recipe.Id}", recipe);

    /*var title = request.Title;
    var ingredients = request.Ingredients;
    var recipeEntity = new Recipe() { Title = title };
    recipeEntity.Ingredients.Select(x => request.Ingredients);
    await context.Recipes.AddAsync(recipeEntity);
    await context.SaveChangesAsync();
    return Results.Ok($"{ recipeEntity.Id.ToString() }, { recipeEntity.Title }");*/
});

app.MapGet("recipe", async (ServiceContext context) =>
{
    var recipeList = await context.Recipes.ToListAsync();
    return Results.Ok(recipeList.Select(recipe => new GetRecipeResult()
    {
        Id = recipe.Id,
        Title = recipe.Title,
        Ingredients = recipe.Ingredients.Select(x => new IngredientResult()
        {
            Id = x.Id,
            Name = x.Name,
        }),
    }));
});

app.MapGet("recipe/{id}", async (ServiceContext context, Guid id) =>
{
    var recipe = await context.Recipes
        .Include(i => i.Ingredients)
        .FirstOrDefaultAsync(x => x.Id == id);
    
    Results.Ok(new GetRecipeResult()
    {
        Id = recipe.Id,
        Title = recipe.Title,
        Ingredients = new List<IngredientResult>(recipe.Ingredients
            .Select(x => new IngredientResult()
        {
            Id = x.Id,
            Name = x.Name,
        }))
    });
});



app.MapPut("recipe/{id}", async (ServiceContext context, Guid id, CreateRecipeRequest request) =>
{
    var recipe = await context.Recipes.FirstOrDefaultAsync(recipe => recipe.Id == id);
    recipe.Title = request.Title;
    await context.SaveChangesAsync();
    Results.Ok(new GetRecipeResult()
    {
        Id = recipe.Id,
        Title = recipe.Title
    });
});

app.MapDelete("recipe/{id}", async (ServiceContext context, Guid id) =>
{
    var recipeToRemove = await context.Recipes.FirstOrDefaultAsync(x => x.Id == id) ?? null;
    context.Recipes.Remove(recipeToRemove!);
    await context.SaveChangesAsync();
    return Results.Ok($"Recipe {recipeToRemove} deleted!");
});

var port = Environment.GetEnvironmentVariable("PORT");
app.Run($"https://localhost:{port}");