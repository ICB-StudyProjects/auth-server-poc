namespace MyApp.OAuth
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using MyApp.OAuth.Configuration;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;
    using MyApp.OAuth.Data.Migrations.IdentityServer;

    //using MyApp.Web.Data.Migrations.IdentityServer.PersistedGrantDb;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var authDbConnectionString = Configuration.GetConnectionString("MyAppOAuthContext");
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services
                .AddIdentityServer()
                //.AddDeveloperSigningCredential()
                .AddSigningCredential(new X509Certificate2(Directory.GetCurrentDirectory() + @"\Certificates\awesomenetwork.pfx", "test"))
                .AddTestUsers(InMemoryConfiguration.Users())
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(authDbConnectionString,
                            sql => sql.MigrationsAssembly(assembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(authDbConnectionString,
                            sql => sql.MigrationsAssembly(assembly));

                    // this enables automatic token cleanup. this is optional.
                    //options.EnableTokenCleanup = true;
                    //options.TokenCleanupInterval = 30;
                });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

            IdentityServerDatabaseInitialization.InitializeDatabase(app);
        }
    }
}
