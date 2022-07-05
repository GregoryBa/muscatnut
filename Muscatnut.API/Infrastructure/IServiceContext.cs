using Microsoft.EntityFrameworkCore;

namespace RecipeService.Infrastructure;

public interface IServiceContext : IDisposable
{
    DbSet<Recipe> Set<Recipe>() where Recipe : class;

    Task<int> SaveChangesAsync();
}