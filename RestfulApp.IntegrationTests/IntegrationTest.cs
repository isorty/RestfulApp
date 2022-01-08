using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Responses;
using RestfulApp.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RestfulApp.IntegrationTests;

public class IntegrationTest
{
    protected readonly HttpClient TestClient;

    protected IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Production");
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<DataContext>));
                    services.AddDbContext<DataContext>(setup =>
                    {
                        setup.UseInMemoryDatabase("TestRestfulAppDb");
                    });
                });
            });
        TestClient = appFactory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
    }

    protected async Task<ItemResponse> CreateItemAsync(CreateItemRequest request)
    {
        var response = await TestClient.PostAsJsonAsync(ApiRoutes.Items.Create, request);
        return await response.Content.ReadFromJsonAsync<ItemResponse>();
    }

    private async Task<string> GetJwtAsync()
    {
        var response = await TestClient.PostAsJsonAsync(
            ApiRoutes.Identity.Register, 
            new UserRegistrationRequest
            {
                Email = "test@integration.com",
                Password = "OneTwoThree4!"
            });

        var registrationResponse = await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();

        return registrationResponse.Token;
    }
}