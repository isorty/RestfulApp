using Microsoft.Net.Http.Headers;
using Refit;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Requests.Queries;
using RestfulApp.Contracts.V1.Responses;

namespace RestfulApp.Sdk;

[Headers($"{HeaderNames.Authorization} : Bearer")]
public interface IItemApi
{
    [Get($"/{ApiRoutes.Items.GetAll}")]
    Task<ApiResponse<PaginatedResponse<ItemResponse>>> GetAllAsync();

    [Get($"/{ApiRoutes.Items.GetAll}")]
    Task<ApiResponse<PaginatedResponse<ItemResponse>>> GetAllAsync([Query] PaginationQuery paginationQuery);

    [Get($"/{ApiRoutes.Items.Get}")]
    Task<ApiResponse<Response<ItemResponse>>> GetAsync(Guid itemId);

    [Post($"/{ApiRoutes.Items.Create}")]
    Task<ApiResponse<Response<ItemResponse>>> CreateAsync([Body] CreateItemRequest createItemRequest);

    [Put($"/{ApiRoutes.Items.Update}")]
    Task<ApiResponse<Response<ItemResponse>>> UpdateAsync(Guid itemId, [Body] UpdateItemRequest updateItemRequest);

    [Delete($"/{ApiRoutes.Items.Delete}")]
    Task<ApiResponse<string>> DeleteAsync(Guid itemId);
}