namespace MyApp.OAuth.Configuration
{
    using IdentityServer4;
    using IdentityServer4.Models;
    using IdentityServer4.Test;
    using System.Collections.Generic;

    public class InMemoryConfiguration
    {
        public static string IdentityServerResources { get; private set; }

        public static IEnumerable<ApiResource> ApiResources()
        {
            return new []
            {
                new ApiResource("myapp", "My App")
            };
        }

        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "myapp",
                    ClientSecrets = new [] { new Secret("$3cr37".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "myapp" }
                },
                new Client
                {
                    ClientId = "myapp_implicit",
                    ClientName = "MyApp Client",
                    //ClientSecrets = new [] { new Secret("$3cr37".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new [] 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "myapp"
                    },
                    RedirectUris = { "http://localhost:57692/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:57692/signin-callback-oidc" }
                }
            };
        }

        public static List<TestUser> Users()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "tester@testing.com",
                    Password = "super_pass"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "my@account.com",
                    Password = "super_pass"
                }
            };
        }
    }
}
