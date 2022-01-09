using Refit;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Responses;

namespace RestfulApp.Sdk;

[Headers("Authorization: Bearer")]
public interface IItemApi
{
    [Get("/api/v1/items")]
    Task<ApiResponse<List<ItemResponse>>> GetAllAsync();

    [Get("/api/v1/items/{itemId}")]
    Task<ApiResponse<ItemResponse>> GetAsync(Guid itemId);

    [Post("/api/v1/items")]
    Task<ApiResponse<ItemResponse>> CreateAsync([Body] CreateItemRequest createItemRequest);

    [Put("/api/v1/items/{itemId}")]
    Task<ApiResponse<ItemResponse>> UpdateAsync(Guid itemId, [Body] UpdateItemRequest updateItemRequest);

    [Delete("/api/v1/items/{itemId}")]
    Task<ApiResponse<string>> DeleteAsync(Guid itemId);
}