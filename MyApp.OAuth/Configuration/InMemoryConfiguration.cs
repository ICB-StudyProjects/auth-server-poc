namespace MyApp.OAuth.Configuration
{
    using IdentityServer4.Models;
    using IdentityServer4.Test;
    using System.Collections.Generic;

    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                new ApiResource("myapp", "My App")
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
