using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestfulApp.Api.Domain;
using RestfulApp.Api.Helpers;
using RestfulApp.Api.Services;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Requests.Queries;
using RestfulApp.Contracts.V1.Responses;
using RestfulApp.Api.Filters;
using System.Net.Mime;

namespace RestfulApp.Api.Controllers.V1;

[Produces(MediaTypeNames.Application.Json)]
public class ItemController : ApiController
{
    private readonly IItemService _itemService;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;
    private readonly IIdentityService _identityService;

    public ItemController(IItemService itemService, IMapper mapper, IUriService uriService, IIdentityService identityService)
    {
        _itemService = itemService;
        _mapper = mapper;
        _uriService = uriService;
        _identityService = identityService;
    }

    /// <summary>
    /// Returns all the items.
    /// </summary>
    /// <response code="200">Successfully returned all the items.</response>
    /// <response code="401">Unauthorized.</response>
    [HttpGet(ApiRoutes.Items.GetAll)]
    [ApiKeyAuth(false)]
    [ProducesResponseType(typeof(PaginatedResponse<ItemResponse>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllItemsQuery query, [FromQuery] PaginationQuery paginationQuery)
    {
        var filter = _mapper.Map<GetAllItemsFilter>(query);

        var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

        var items = await _itemService.GetItemsAsync(filter, pagination);

        var itemResponses = _mapper.Map<List<ItemResponse>>(items);

        if (pagination is null ||
            pagination.PageNumber < 1 ||
            pagination.PageSize < 1)
        {
            return Ok(new PaginatedResponse<ItemResponse>(itemResponses));
        }

        var paginatedResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, itemResponses);

        return Ok(paginatedResponse);
    }

    /// <summary>
    /// Returns an item by its id.
    /// </summary>
    /// <response code="200">Successfully returned an item by its id.</response>
    /// <response code="404">An item with given id was not found.</response>
    [HttpGet(ApiRoutes.Items.Get)]
    [ProducesResponseType(typeof(Response<ItemResponse>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> GetAsync(Guid itemId)
    {
        var item = await _itemService.GetItemByIdAsync(itemId);
        var itemResponse = _mapper.Map<ItemResponse>(item);
        var response = new Response<ItemResponse>(itemResponse);

        return item is not null ?
            Ok(response) :
            NotFound();
    }

    /// <summary>
    /// Creates a new item.
    /// </summary>
    /// <response code="201">Successfully created a new item.</response>
    /// <response code="400">Validation error occurred.</response>
    [HttpPost(ApiRoutes.Items.Create)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Response<ItemResponse>), 201)]
    [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
    public async Task<IActionResult> CreateAsync(CreateItemRequest itemRequest)
    {
        var newItemId = Guid.NewGuid();
        var user = HttpContext.User;

        var item = _mapper.Map<Item>(itemRequest);
        item.Id = newItemId;
        item.UserId = _identityService.GetUserId();

        var created = await _itemService.CreateItemAsync(item);

        if (!created)
        {
            return BadRequest();
        }

        var locationUri = _uriService.GetItemUri(item.Id.ToString());

        var itemResponse = _mapper.Map<ItemResponse>(item);
        var response = new Response<ItemResponse>(itemResponse);

        return Created(locationUri, response);
    }

    /// <summary>
    /// Updates an item by its id.
    /// </summary>
    /// <response code="200">Successfully updated an item by its id.</response>
    /// <response code="400">Validation error occurred.</response>
    /// <response code="404">An item with given id was not found.</response>
    [HttpPut(ApiRoutes.Items.Update)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Response<ItemResponse>), 200)]
    [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> UpdateAsync(Guid itemId, UpdateItemRequest updateItemRequest)
    {
        var userId = _identityService.GetUserId();

        if (!await _itemService.UserOwnsItemAsync(itemId, userId))
        {
            return NotFound();
        }

        var item = _mapper.Map<Item>(updateItemRequest);
        item.Id = itemId;

        var updatedItem = await _itemService.UpdateItemAsync(item);

        if (updatedItem is null)
        {
            return NotFound();
        }

        var updatedItemRespone = _mapper.Map<ItemResponse>(updatedItem);

        return Ok(new Response<ItemResponse> { Data = updatedItemRespone });
    }

    /// <summary>
    /// Deletes an item by its id.
    /// </summary>
    /// <response code="204">Successfully deleted an item by its id.</response>
    /// <response code="404">An item with given id was not found.</response>
    [HttpDelete(ApiRoutes.Items.Delete)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> DeleteAsync(Guid itemId)
    {
        var userId = _identityService.GetUserId();

        if (!await _itemService.UserOwnsItemAsync(itemId, userId))
        {
            return NotFound();
        }

        return await _itemService.DeleteItemAsync(itemId) ?
            NoContent() :
            NotFound();
    }
}