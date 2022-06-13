using Microsoft.EntityFrameworkCore;
using RecipeService;
using RecipeService.Entities;
using RecipeService.Features;
using RecipeService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ServiceContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnectionString"))
    );

var app = builder.Build();
// Middleware
app.UseSwagger();
app.UseSwaggerUI();

// Endpoints

app.MapGet("recipe", async (ServiceContext context) =>
{
    var recipeList = await context.Recipes.ToListAsync();
    return Results.Ok(recipeList.Select(recipe => new GetRecipeResult()
    {
        Id = recipe.Id,
        Title = recipe.Title
    }));
});

app.MapGet("recipe/{id}", async (ServiceContext context, Guid id) =>
{
    var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id == id);
    
    Results.Ok(new GetRecipeResult()
    {
        Id = recipe.Id,
        Title = recipe.Title
        
    });
});

app.MapPost("recipe", async (ServiceContext context, CreateRecipeRequest request) =>
{
    var title = request.Title;
    var ingredients = request.Ingredients;
    var recipeEntity = new Recipe() { Title = title };
    await context.Recipes.AddAsync(recipeEntity);
    await context.SaveChangesAsync();
    return Results.Ok($"{ recipeEntity.Id.ToString() }, { recipeEntity.Title }");
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