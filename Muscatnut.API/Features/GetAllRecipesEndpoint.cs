/*
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Muscatnut.API.Infrastructure;

namespace Muscatnut.API.Features;


public class GetAllRecipesEndpoint : Endpoint<GetAllRecipesRequest, GetRecipeResult>
{
    public ServiceContext Context;

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("recipe");
        AllowAnonymous();
    }

    public override async Task<GetRecipeResult> HandleAsync(GetAllRecipesRequest req, CancellationToken cancellationToken)
    {
        var recipe = await Context.Recipes.FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return new GetRecipeResult
        {
            Id = recipe.Id,
            Title = recipe.Title
        };
    }
}
*/

