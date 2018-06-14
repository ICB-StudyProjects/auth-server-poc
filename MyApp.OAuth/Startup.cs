namespace MyApp.OAuth
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using MyApp.OAuth.Configuration;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddIdentityServer()
                //.AddDeveloperSigningCredential()
                .AddSigningCredential(new X509Certificate2(Directory.GetCurrentDirectory() + @"\Certificates\awesomenetwork.pfx", "test"))
                .AddInMemoryIdentityResources(InMemoryConfiguration.IdentityResources())
                .AddInMemoryApiResources(InMemoryConfiguration.ApiResources())
                .AddInMemoryClients(InMemoryConfiguration.Clients())
                .AddTestUsers(InMemoryConfiguration.Users());

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
        }
    }
}
