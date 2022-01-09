using FluentAssertions;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace RestfulApp.IntegrationTests;

public class ItemControllerTests : IntegrationTest
{
    [Fact]
    public async Task Returns_Token_When_Register_New_User()
    {
        //Arrange


        //Act
        var authResponse = await AuthenticateAsync();

        //Assert
        authResponse.Should().NotBeNull();
        authResponse.Token.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllAsync_WithoutAnyItems_ReturnsEmpty()
    {
        //Arange
        _ = await AuthenticateAsync();
        var requestUri = ApiRoutes.Items.GetAll;

        //Act
        var response = await TestClient.GetAsync(requestUri);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<List<Item>>()).Should().BeEmpty();
    }

    [Fact]
    public async Task GetAsync_ReturnsItem_WhenItemExistsInDb()
    {
        //Arrange        
        _ = await AuthenticateAsync();
        var guid = Guid.NewGuid().ToString();
        var createdItem = await CreateItemAsync(new CreateItemRequest { Name = guid });
        var requestUri = ApiRoutes.Items.Get.Replace("{itemId}", createdItem.Id.ToString());

        //Act
        var response = await TestClient.GetAsync(requestUri);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returnedItem = await response.Content.ReadFromJsonAsync<Item>();
        returnedItem.Id.Should().Be(createdItem.Id);
        returnedItem.Name.Should().Be(guid);
    }
}