namespace MyApp.OAuth.Configuration
{
    using IdentityServer4;
    using IdentityServer4.Models;
    using IdentityServer4.Test;
    using System.Collections.Generic;
    using System.Security.Claims;

    public class InMemoryConfiguration
    {
        public static string IdentityServerResources { get; private set; }

        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                new ApiResource("myapp", "My App")
            };
        }

        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Address()
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
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        "myapp"
                    },
                    AllowAccessTokensViaBrowser = true, // Should be {false}
                    RedirectUris = { "http://localhost:57692/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:57692/signin-callback-oidc" }
                },
                new Client
                {
                    ClientId = "myapp_code",
                    ClientName = "MyApp Code Flow",
                    ClientSecrets = new [] { new Secret("$3cr37".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        "myapp"
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true, // Should be {false}
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
                    Password = "super_pass",
                    Claims = new []
                    {
                        new Claim("name", "Tester"),
                        new Claim("website", "https://testing.com"),
                        new Claim("email", "tester@testing.com"),
                        new Claim("address", "24 Somewhere str."),
                        new Claim("phone", "+359 123 123 123")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "my@account.com",
                    Password = "super_pass",
                    Claims = new [] 
                    {
                        new Claim("name", "MyName"),
                        new Claim("website", "https://website.com"),
                        new Claim("email", "my@account.com"),
                        new Claim("address", "24 Somewhere str."),
                        new Claim("phone", "+359 312 312 313")
                    }
                }
            };
        }
    }
}
