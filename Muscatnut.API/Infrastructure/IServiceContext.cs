using Microsoft.EntityFrameworkCore;
using RecipeService.Entities;

namespace RecipeService.Infrastructure;

public interface IServiceContext : IDisposable
{
    DbSet<Recipe> Set<Recipe>() where Recipe : class;

    Task<int> SaveChangesAsync();
}