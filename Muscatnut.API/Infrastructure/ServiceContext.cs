using Microsoft.EntityFrameworkCore;
using RecipeService.Models;

namespace RecipeService.Infrastructure;

public class ServiceContext : DbContext
{
    public DbSet<RecipeEntity> Recipes { get; set; }
    
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