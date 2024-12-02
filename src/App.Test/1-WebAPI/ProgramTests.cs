using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace APICliente.Tests
{


    internal class MoroniWebApplication : WebApplicationFactory<Program>
    {
        public MoroniWebApplication()
        {
            var myConfiguration = new Dictionary<string, string>
                {
                    {"CognitoClientId", "Value1"},
                    {"CognitoUserPoolId", "NestedValue1"},
                    {"CognitoClientSecret", "NestedValue2"},
                    {"AWS_ACCESS_KEY_ID", "NestedValue2"},
                    {"AWS_SECRET_ACCESS_KEY", "NestedValue2"}
                };

            MyConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }
        public IConfiguration MyConfiguration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
           

            _ = builder.ConfigureTestServices(services =>
            {
                
                services.AddSingleton<IConfiguration>(MyConfiguration);
            });
        }
    }
   
public class ProgramTests
    {
        [Fact]
        public async Task ShoudReturns200_WhenGetValidPath()
        {
            // Arrange
            await using var application = new MoroniWebApplication();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/clientes/21956211");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }


}
