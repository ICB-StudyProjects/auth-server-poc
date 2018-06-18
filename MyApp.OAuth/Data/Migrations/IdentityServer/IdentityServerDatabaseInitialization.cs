namespace MyApp.OAuth.Data.Migrations.IdentityServer
{
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Mappers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using MyApp.OAuth.Configuration;
    using System.Linq;

    public class IdentityServerDatabaseInitialization
    {
        public static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                                         .GetService<IServiceScopeFactory>()
                                         .CreateScope())
            {
                PerformMigrations(serviceScope);
                SeedData(serviceScope);
            }
        }

        private static void PerformMigrations(IServiceScope serviceScope)
        {
            serviceScope.ServiceProvider
                        .GetRequiredService<ConfigurationDbContext>()
                        .Database
                        .Migrate();

            serviceScope.ServiceProvider
                        .GetRequiredService<PersistedGrantDbContext>()
                        .Database
                        .Migrate();
        }

        private static void SeedData(IServiceScope serviceScope)
        {
            var context = serviceScope
                           .ServiceProvider
                           .GetRequiredService<ConfigurationDbContext>();

            if (!context.Clients.Any())
            {
                foreach (var client in InMemoryConfiguration.Clients())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in InMemoryConfiguration.IdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in InMemoryConfiguration.ApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}
