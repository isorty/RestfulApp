using Refit;
using RestfulApp.Contracts.V1.Requests;
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
(await itemApi.GetAllAsync()).Content.ForEach(item => Console.WriteLine(ToJson(item)));
Console.WriteLine();

var createdItem = await itemApi.CreateAsync(new CreateItemRequest
{
    Name = "New sdk post"
});

var retrievedNewItem = await itemApi.GetAsync(createdItem.Content.Id);

Console.WriteLine("Created item:");
Console.WriteLine(ToJson(retrievedNewItem.Content));
Console.WriteLine();
Console.WriteLine("All Items:");
(await itemApi.GetAllAsync()).Content.ForEach(item => Console.WriteLine(ToJson(item)));
Console.WriteLine();
Console.WriteLine("Created item deleted.");
_ = await itemApi.DeleteAsync(retrievedNewItem.Content.Id);
Console.WriteLine("All Items:");
(await itemApi.GetAllAsync()).Content.ForEach(item => Console.WriteLine(ToJson(item)));

Console.ReadKey();
static string ToJson(object o) => JsonSerializer.Serialize(o, new JsonSerializerOptions { WriteIndented = true });