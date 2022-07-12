using FluentValidation;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using RecipeService.Contracts;
using RecipeService.Infrastructure;
using RecipeService.Models;
using RecipeService.Services;

var builder = WebApplication.CreateBuilder(args);
// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHashids>(_ => new Hashids("g5GPjxX0py", 11));
builder.Services.AddDbContext<ServiceContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnectionString") ?? string.Empty)
    );

builder.Services.AddScoped<IRecipeService, RecipeService.Services.RecipeService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

// Endpoints
app.MapPost("recipe", async (CreateRecipeRequest recipe, IRecipeService recipeService) =>
{
    var hashedId = await recipeService.CreateAsync(new RecipeEntity()
    {
        Description = recipe.Description,
        Title = recipe.Title,
        Ingredients = recipe.Ingredients.Select(x => new IngredientEntity()
        {
            Name = x.Name
        }).ToList()
    });
    if (hashedId.Length == 0)
    {
        return Results.BadRequest(new
        {
            errorMessage = "Something went wrong when creating the recipe"
        });
    }

    return Results.Created($"/recipe/{hashedId}", recipe);
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

/*
app.MapGet("recipe/{id}", async (ServiceContext context, string id) =>
{
    /*var rawId = hashids.Decode(id);

    if (rawId.Length == 0)
    {
        Results.NotFound();
    }#1#
    
    /*var recipe = await context.Recipes
        .Include(i => i.Ingredients)
        .FirstOrDefaultAsync(x => x.Id == id);#1#

    //**
    /*Results.Ok(recipe); #1#/*new GetRecipeResult()
    {
        Id = recipe.Id,#2#
        Title = recipe.Title,
        Ingredients = new List<IngredientResult>(recipe.Ingredients
            .Select(x => new IngredientResult()
        {
            Id = x.Id,
            Name = x.Name,
        }))
    });#2#
});
#1#
*/



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