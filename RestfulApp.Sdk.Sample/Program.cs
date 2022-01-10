using Refit;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Requests.Queries;
using RestfulApp.Sdk;
using System.Text.Json;

var email = "sdk@test.com";
var password = "Sdk1234!";

var cachedToken = string.Empty;

var identityApi = RestService.For<IIdentityApi>("https://localhost:7151");
var itemApi = RestService.For<IItemApi>("https://localhost:7151", new()
{
    AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
});

//var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
//{
//    Email = email,
//    Password = password
//});

var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
{
    Email = email,
    Password = password
});

cachedToken = loginResponse.Content.Token;

Console.WriteLine("All Items:");
var itemsResponse = await itemApi.GetAllAsync();
Console.WriteLine(ToJson(itemsResponse.Content));
Console.WriteLine();

Console.WriteLine("All Items with pagination:");
var paginationQuery = new PaginationQuery(2, 2);
var paginatedItemsResponse = await itemApi.GetAllAsync(paginationQuery);
Console.WriteLine(ToJson(paginatedItemsResponse.Content));
Console.WriteLine();

var createdItemResponse = await itemApi.CreateAsync(new CreateItemRequest
{
    Name = "New sdk post"
});

var retrievedNewItemResponse = await itemApi.GetAsync(createdItemResponse.Content.Data.Id);

Console.WriteLine("Created item:");
Console.WriteLine(ToJson(retrievedNewItemResponse.Content));
Console.WriteLine();
Console.WriteLine("All Items:");
Console.WriteLine(ToJson((await itemApi.GetAllAsync()).Content));
Console.WriteLine();
Console.WriteLine("Created item deleted.");
_ = await itemApi.DeleteAsync(retrievedNewItemResponse.Content.Data.Id);
Console.WriteLine("All Items:");
Console.WriteLine(ToJson((await itemApi.GetAllAsync()).Content));

Console.ReadKey();
static string ToJson(object o) => JsonSerializer.Serialize(o, new JsonSerializerOptions { WriteIndented = true });