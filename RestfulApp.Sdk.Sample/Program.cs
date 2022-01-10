using Refit;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Requests.Queries;
using RestfulApp.Sdk;

var email = "sdk@test.com";
var password = "Sdk1234!";

var identityApi = RestService.For<IIdentityApi>("https://localhost:7151");

var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
{
    Email = email,
    Password = password
});

var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
{
    Email = email,
    Password = password
});

var itemApi = RestService.For<IItemApi>("https://localhost:7151", new()
{
    AuthorizationHeaderValueGetter = () => Task.FromResult(loginResponse.Content is not null ? loginResponse.Content.Token : string.Empty)
});


var getAllItemsResponse = await itemApi.GetAllAsync();

var getAllItemsPaginatedResponse = await itemApi.GetAllAsync(new PaginationQuery(1, 10));

var createItemResponse = await itemApi.CreateAsync(new CreateItemRequest { Name = "Sdk Item" });

var updateItemResponse = await itemApi.UpdateAsync(createItemResponse.Content.Data.Id, new UpdateItemRequest { Name = "Updated Sdk Item" });

var deleteItemResponse = await itemApi.DeleteAsync(createItemResponse.Content.Data.Id);