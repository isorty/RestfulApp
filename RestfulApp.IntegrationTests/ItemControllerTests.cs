using FluentAssertions;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Security.Permissions;
using System.Threading.Tasks;
using Xunit;

namespace RestfulApp.IntegrationTests;

public class ItemControllerTests : IntegrationTest
{
    [Fact]
    public async Task GetAllAsync_WithoutAnyItems_ReturnsEmpty()
    {
        //Arange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync(ApiRoutes.Items.GetAll);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<List<Item>>()).Should().BeEmpty();
    }

    [Fact]
    public async Task GetAsync_ReturnsItem_WhenItemExistsInDb()
    {
        //Arrange
        await AuthenticateAsync();
        var guid = Guid.NewGuid().ToString();
        var createdItem = await CreateItemAsync(new CreateItemRequest { Name = guid });
        //Act
        var response = await TestClient.GetAsync(ApiRoutes.Items.Get.Replace("{itemId}", createdItem.Id.ToString()));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returnedItem = await response.Content.ReadFromJsonAsync<Item>();
        returnedItem.Id.Should().Be(createdItem.Id);
        returnedItem.Name.Should().Be(guid);
    }
}