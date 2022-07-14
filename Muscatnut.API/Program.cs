using FluentValidation;
using FluentValidation.Results;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RecipeService.Auth;
using RecipeService.Contracts;
using RecipeService.Infrastructure;
using RecipeService.Models;
using RecipeService.Services;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    WebRootPath = "./wwwroot",
    EnvironmentName = Environment.GetEnvironmentVariable("env"),
    ApplicationName = "Muscatnut.API",
    
});


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
                return
                    Results.BadRequest(validationResult
                        .Errors); // You might need to return custom contract here instead of validation result
            }

            var hashedId = await recipeService.CreateAsync(recipeRequest);
            if (hashedId.Length == 0)
            {
                return Results.BadRequest(new
                {
                    errorMessage = "Something went wrong when creating the recipe"
                });
            }

            return Results.CreatedAtRoute("GetRecipe", new { Id = hashedId }, recipeRequest);
        })
    .WithName("CreateRecipe")
    .Accepts<CreateRecipeRequest>("application/json")
    .Produces<CreateRecipeRequest>(201)
    .Produces<IEnumerable<ValidationFailure>>();

app.MapGet("recipe",
    async (IRecipeService recipeService, string? searchTerm) =>
{
    
    if (searchTerm is not null && !string.IsNullOrWhiteSpace(searchTerm))
    {
        var matchedRecipes = await recipeService.SearchByTitleAsync(searchTerm);
        return Results.Ok(matchedRecipes);
    }
    var recipes = await recipeService.GetAllAsync();
    return Results.Ok(recipes);
}).WithName("GetRecipe")
    .Produces<IEnumerable<RecipeEntity>>()
    .Produces<RecipeEntity>(200)
    .Produces(404);

app.MapPut("recipe/{id}", 
    [Authorize(AuthenticationSchemes = ApiKeySchemeConstants.SchemeName)]
    async (IRecipeService recipeService, string id, CreateRecipeRequest recipeRequest) =>
{
    var result = await recipeService.UpdateAsync(recipeRequest, id);
    if (result == false) return Results.BadRequest("Coudln't find the recipe");
    return Results.Ok("Recipe updated");
})
    .WithName("UpdateRecipe")
    .Accepts<CreateRecipeRequest>("application/json")
    .Produces<CreateRecipeRequest>(200)
    .Produces<IEnumerable<ValidationFailure>>(400);;

app.MapDelete("recipe/{id}",
    [Authorize(AuthenticationSchemes = ApiKeySchemeConstants.SchemeName)]
    async (IRecipeService recipeService, string id) =>
{
    var result = await recipeService.DeleteAsync(id);
    if (result == false) return Results.BadRequest("Coudln't find the recipe");
    return Results.Ok("Recipe updated");
})
    .WithName("DeleteRecipe")
    .Accepts<CreateRecipeRequest>("application/json")
    .Produces<CreateRecipeRequest>(204)
    .Produces<IEnumerable<ValidationFailure>>(404);;

var port = Environment.GetEnvironmentVariable("PORT");
app.Run($"https://localhost:{port}");