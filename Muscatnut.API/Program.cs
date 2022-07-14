using FluentValidation;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RecipeService.Auth;
using RecipeService.Contracts;
using RecipeService.Infrastructure;
using RecipeService.Services;

var builder = WebApplication.CreateBuilder(args);
// Services
builder.Services.AddAuthentication(ApiKeySchemeConstants.SchemeName)
    .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthHandler>(ApiKeySchemeConstants.SchemeName, _ => { });
builder.Services.AddAuthorization();

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

app.UseAuthorization();

// Endpoints
app.MapPost("recipe", 
    [Authorize(AuthenticationSchemes = ApiKeySchemeConstants.SchemeName)]
    async (CreateRecipeRequest recipeRequest, IRecipeService recipeService,
    IValidator<CreateRecipeRequest> validator) =>
{
    var validationResult = await validator.ValidateAsync(recipeRequest);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors); // You might need to return custom contract here instead of validation result
    }

    var hashedId = await recipeService.CreateAsync(recipeRequest);
    if (hashedId.Length == 0)
    {
        return Results.BadRequest(new
        {
            errorMessage = "Something went wrong when creating the recipe"
        });
    }

    return Results.Created($"/recipe/{hashedId}", recipeRequest);
});

app.MapGet("recipe", async (IRecipeService recipeService, string? searchTerm) =>
{
    if (searchTerm is not null && !string.IsNullOrWhiteSpace(searchTerm))
    {
        var matchedRecipes = await recipeService.SearchByTitleAsync(searchTerm);
        return Results.Ok(matchedRecipes);
    }
    var recipes = await recipeService.GetAllAsync();
    return Results.Ok(recipes);
});

app.MapPut("recipe/{hashedId}", async (IRecipeService recipeService, string hashedId, CreateRecipeRequest recipeRequest) =>
{
    var result = await recipeService.UpdateAsync(recipeRequest, hashedId);
    if (result == false) return Results.BadRequest("Coudln't find the recipe");
    return Results.Ok("Recipe updated");
});

app.MapDelete("recipe/{hashedId}", async (IRecipeService recipeService, string hashedId) =>
{
    var result = await recipeService.DeleteAsync(hashedId);
    if (result == false) return Results.BadRequest("Coudln't find the recipe");
    return Results.Ok("Recipe updated");
});

var port = Environment.GetEnvironmentVariable("PORT");
app.Run($"https://localhost:{port}");