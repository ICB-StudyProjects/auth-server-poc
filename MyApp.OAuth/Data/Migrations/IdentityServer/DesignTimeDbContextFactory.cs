namespace MyApp.OAuth.Data.Migrations.IdentityServer
{
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using System.Reflection;

    // HACK: Change to "PersistedGrantDbContext" or "ConfigurationDbContext", change the store and run the cli command
    public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();

            var connStr = configuration.GetConnectionString("MyAppOAuthContext");
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            builder.UseSqlServer(connStr, o => o.MigrationsAssembly(assembly));

            return new ConfigurationDbContext(builder.Options, new ConfigurationStoreOptions());
        }

        //ConfigurationDbContext IDesignTimeDbContextFactory<ConfigurationDbContext>.CreateDbContext(string[] args)
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json")
        //    .AddEnvironmentVariables()
        //    .Build();

        //    var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();

        //    var connStr = configuration.GetConnectionString("MyAppOAuthContext");
        //    var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

        //    builder.UseSqlServer(connStr, o => o.MigrationsAssembly(assembly));

        //    return new ConfigurationDbContext(builder.Options, new ConfigurationStoreOptions());
        //}
    }
}
