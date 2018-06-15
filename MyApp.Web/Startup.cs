namespace MyApp.Web
{
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = "http://localhost:51364/";
                    options.RequireHttpsMetadata = false; // {true} in Production

                    // When a client only requests response_type=id_token (which means no API is being used) then all the claims for the identity scopes requested go into the id_token. When the client requests response_type=id_token token, then (by default) only the sub claim goes into the id_token and the rest of the identity claims are returned via the userinfo endpoint.
                    // Added "token" to get "access token" back from the auth server
                    //options.ResponseType = "id_token"; // if "token" to get the "access_token" then only "sub" claims goes to the "id_token" 
                    //options.ClientId = "myapp_implicit";

                    // Authorization Code Flow Conf
                    options.ResponseType = "id_token code"; // Same goes for the claims as stated above
                    options.ClientId = "myapp_code";
                    options.ClientSecret = "$3cr37";
                    options.Scope.Add("myapp");
                    options.Scope.Add("offline_access");
                    options.GetClaimsFromUserInfoEndpoint = true;

                    // Additional user scopes
                    options.Scope.Add("email");
                    options.Scope.Add("phone");
                    options.Scope.Add("address");

                    options.SaveTokens = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
