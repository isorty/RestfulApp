using FluentAssertions;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Responses;
using System;
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

    [Fact]
    public async Task GetAsync_ReturnsPaginatedItems_WhenItemsExistsInDb()
    {
        //Arrange        
        _ = await AuthenticateAsync();
        var name = "Test Name";
        var createItemRequest = new CreateItemRequest { Name = name };
        _ = await CreateItemAsync(createItemRequest);
        _ = await CreateItemAsync(createItemRequest);
        _ = await CreateItemAsync(createItemRequest);
        _ = await CreateItemAsync(createItemRequest);
        _ = await CreateItemAsync(createItemRequest);
        _ = await CreateItemAsync(createItemRequest);

        var paginatedQuery = "?pageNumber={0}&pageSize={1}";

        var requestUri = $"{ApiRoutes.Items.GetAll}{string.Format(paginatedQuery, 2, 2)}";

        //Act
        var response = await TestClient.GetAsync(requestUri);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var returnedPaginatedItemResponse = await response.Content.ReadFromJsonAsync<PaginatedResponse<ItemResponse>>();
        returnedPaginatedItemResponse.Data.Should().HaveCount(2);
        returnedPaginatedItemResponse.PageNumber.Should().Be(2);
        returnedPaginatedItemResponse.PageSize.Should().Be(2);
        returnedPaginatedItemResponse.NextPage.Should().Be($"{TestClient.BaseAddress}{ApiRoutes.Items.GetAll}{string.Format(paginatedQuery, 3, 2)}");
        returnedPaginatedItemResponse.PreviousPage.Should().Be($"{TestClient.BaseAddress}{ApiRoutes.Items.GetAll}{string.Format(paginatedQuery, 1, 2)}");
    }

    [Fact]
    public async Task GetAsync_ReturnsUpdatedItem_WhenItemUpdatedInDb()
    {
        //Arrange        
        _ = await AuthenticateAsync();
        var name = "Test Name";
        var newName = "New Name";
        var createdItemResponse = await CreateItemAsync(new CreateItemRequest { Name = name });
        var createdItemId = createdItemResponse.Data.Id;

        var getRequestUri = ApiRoutes.Items.Get.Replace("{itemId}", createdItemId.ToString());
        var updateRequestUri = ApiRoutes.Items.Update.Replace("{itemId}", createdItemId.ToString());

        //Act
        var updateItemRequest = new UpdateItemRequest { Name = newName };
        var updatedResponse = await TestClient.PutAsync(updateRequestUri, JsonContent.Create(updateItemRequest));
        var getResponse = await TestClient.GetAsync(getRequestUri);

        //Assert
        updatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedItemResponse = await updatedResponse.Content.ReadFromJsonAsync<Response<ItemResponse>>();
        var getItemResponse = await getResponse.Content.ReadFromJsonAsync<Response<ItemResponse>>();

        updatedItemResponse.Data.Id.Should().Be(createdItemId);
        updatedItemResponse.Data.Name.Should().Be(newName);

        getItemResponse.Data.Id.Should().Be(createdItemId);
        getItemResponse.Data.Name.Should().Be(newName);
    }

    [Fact]
    public async Task GetAsync_ReturnsNoFound_WhenDeleteNotExistingItemInDb()
    {
        //Arrange        
        _ = await AuthenticateAsync();
        var requestUri = ApiRoutes.Items.Delete.Replace("{itemId}", Guid.NewGuid().ToString());

        //Act
        var response = await TestClient.DeleteAsync(requestUri);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAsync_ReturnsNoContent_WhenDeleteItemInDb()
    {
        //Arrange        
        _ = await AuthenticateAsync();
        var name = "Test Name";
        var createdItemResponse = await CreateItemAsync(new CreateItemRequest { Name = name });
        var requestUri = ApiRoutes.Items.Delete.Replace("{itemId}", createdItemResponse.Data.Id.ToString());

        //Act
        var response = await TestClient.DeleteAsync(requestUri);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}