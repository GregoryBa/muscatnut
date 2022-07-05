using Microsoft.EntityFrameworkCore;
using RecipeService;
using RecipeService.Contracts;
using RecipeService.Features;
using RecipeService.Infrastructure;
using RecipeService.Models;
using RecipeService.Services;

var builder = WebApplication.CreateBuilder(args);
// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ServiceContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnectionString") ?? string.Empty)
    );
builder.Services.AddScoped<IRecipeService, RecipeService.Services.RecipeService>();

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

// Endpoints
app.MapPost("recipe", async (CreateRecipeRequest recipe, IRecipeService recipeService) =>
{
    var created = await recipeService.CreateAsync(new RecipeEntity()
    {
        Description = recipe.Description,
        Title = recipe.Title,
        Ingredients = recipe.Ingredients.Select(x => new IngredientEntity()
        {
            Name = x.Name
        })
    });
    if (created == null)
    {
        return Results.BadRequest(new
        {
            errorMessage = "A recipe with this title already exists"
        });
    }

    return Results.Created($"/recipe/{created}", recipe);
});

app.MapGet("recipe", async (ServiceContext context) =>
{
    var recipeList = await context.Recipes.ToListAsync();
    return Results.Ok(recipeList);
    /*.Select(recipe => new GetRecipeResult()
    {
        Id = recipe.Id,
        Title = recipe.Title,
        Ingredients = recipe.Ingredients.Select(x => new IngredientResult()
        {
            Id = x.Id,
            Name = x.Name,
        }),
    }));*/
});

app.MapGet("recipe/{id}", async (ServiceContext context, Guid id) =>
{
    var recipe = await context.Recipes
        .Include(i => i.Ingredients)
        .FirstOrDefaultAsync(x => x.Id == id);

    Results.Ok(recipe); /*new GetRecipeResult()
    {
        Id = recipe.Id,
        Title = recipe.Title,
        Ingredients = new List<IngredientResult>(recipe.Ingredients
            .Select(x => new IngredientResult()
        {
            Id = x.Id,
            Name = x.Name,
        }))
    });*/
});



app.MapPut("recipe/{id}", async (ServiceContext context, Guid id, CreateRecipeRequest request) =>
{
    /*var recipe = await context.Recipes.FirstOrDefaultAsync(recipe => recipe.Id == id);
    recipe.Title = request.Title;
    await context.SaveChangesAsync();
    Results.Ok(new GetRecipeResult()
    {
        Id = recipe.Id,
        Title = recipe.Title
    });*/
});

app.MapDelete("recipe/{id}", async (ServiceContext context, Guid id) =>
{
    /*var recipeToRemove = await context.Recipes.FirstOrDefaultAsync(x => x.Id == id) ?? null;
    context.Recipes.Remove(recipeToRemove!);
    await context.SaveChangesAsync();
    return Results.Ok($"Recipe {recipeToRemove} deleted!");*/
});

var port = Environment.GetEnvironmentVariable("PORT");
app.Run($"https://localhost:{port}");