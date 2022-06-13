using Microsoft.EntityFrameworkCore;
using RecipeService.Entities;

namespace RecipeService.Infrastructure;

public class ServiceContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }

    public DbSet<Ingredient> Ingredients { get; set; }
    
    public ServiceContext(DbContextOptions<ServiceContext> options) : base(options)
    {
        
    }
    
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        
    }

}