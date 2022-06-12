using Azure.Core;
using Microsoft.EntityFrameworkCore;
using RecipeService;
using RecipeService.Entities;
using RecipeService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");

builder.Services.AddDbContext<ServiceContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnectionString"))
    );

var app = builder.Build();
// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("recipe", async (ServiceContext context) => Results.Ok(await context.Recipes.ToListAsync()));
app.MapGet("recipe/{id}", (Guid id) => "Get a specific recipe");
app.MapPost("recipe", async (ServiceContext context, CreateRecipeRequest request) =>
{
    var title = request.Title;
    var ingredients = request.Ingredients;
    var recipeEntity = new Recipe() { Title = title };
    await context.Recipes.AddAsync(recipeEntity);
    context.SaveChangesAsync();
    return Results.Ok($"{recipeEntity.Id}, {recipeEntity.Title}");
});
//     Results.Ok(await context.Recipes.AddAsync(new Recipe()
// {
//     // Ingredients = request.Ingredients, 
//     Title = request.Title
// })));
app.MapPut("recipe/{id}", (Guid id) => "Update a recipe");
app.MapDelete("recipe/{id}", (Guid id) => "Delete a recipe");

var port = Environment.GetEnvironmentVariable("PORT");
app.Run($"https://localhost:{port}");