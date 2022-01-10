using FluentAssertions;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Responses;
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
        var responseConent = await response.Content.ReadFromJsonAsync<PaginatedResponse<ItemResponse>>();
        responseConent.Data.Should().BeEmpty();
        responseConent.PageNumber.Should().Be(1);
        responseConent.PageSize.Should().Be(4000);
        responseConent.NextPage.Should().BeNull();
        responseConent.PreviousPage.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_ReturnsItem_WhenItemExistsInDb()
    {
        //Arrange        
        _ = await AuthenticateAsync();
        var name = "Test Name";
        var createdItemResponse = await CreateItemAsync(new CreateItemRequest { Name = name });
        var requestUri = ApiRoutes.Items.Get.Replace("{itemId}", createdItemResponse.Data.Id.ToString());

        //Act
        var response = await TestClient.GetAsync(requestUri);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returnedItemResponse = await response.Content.ReadFromJsonAsync<Response<ItemResponse>>();
        returnedItemResponse.Data.Id.Should().Be(createdItemResponse.Data.Id);
        returnedItemResponse.Data.Name.Should().Be(name);
    }
}