using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkillCrest_LearningPlatform.Data;

public class DatabaseInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void InitializeDatabase()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Check if database exists, create it if not, and apply migrations
            if (!context.Database.CanConnect())
            {
                Console.WriteLine("Database does not exist. Creating...");
                context.Database.EnsureCreated(); // Create the database if it doesn't exist
                Console.WriteLine("Database created.");
            }
            else
            {
                Console.WriteLine("Database exists. Applying migrations...");
                context.Database.Migrate(); // Apply all pending migrations
                Console.WriteLine("Migrations applied.");
            }
        }
    }
}
