using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Requests.Queries;
using RestfulApp.Contracts.V1.Responses;
using RestfulApp.Domain;
using RestfulApp.Extensions;
using RestfulApp.Filters;
using RestfulApp.Helpers;
using RestfulApp.Services;

namespace RestfulApp.Controllers.V1;

[Produces("application/json")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public ItemController(IItemService itemService, IMapper mapper, IUriService uriService)
    {
        _itemService = itemService;
        _mapper = mapper;
        _uriService = uriService;
    }

    /// <summary>
    /// Returns all the items.
    /// </summary>
    /// <response code="200">Successfully returned all the items.</response>
    /// <response code="401">Unauthorized.</response>
    [HttpGet(ApiRoutes.Items.GetAll)]
    [ApiKeyAuth(false)]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationQuery paginationQuery)
    {
        var paginationFiler = _mapper.Map<PaginationFilter>(paginationQuery);

        var items = await _itemService.GetItemsAsync(paginationFiler);

        var itemResponses = _mapper.Map<List<ItemResponse>>(items);

        if (paginationFiler is null ||
            paginationFiler.PageNumber < 1 ||
            paginationFiler.PageSize < 1)
        {
            return Ok(new PaginatedResponse<ItemResponse>(itemResponses));
        }

        var paginatedResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, paginationFiler, itemResponses);

        return Ok(paginatedResponse);
    }

    [HttpGet(ApiRoutes.Items.Get)]
    public async Task<IActionResult> GetAsync([FromRoute] Guid itemId)
    {
        var item = await _itemService.GetItemByIdAsync(itemId);
        var itemResponse = _mapper.Map<ItemResponse>(item);
        var response = new Response<ItemResponse>(itemResponse);

        return item is not null ? Ok(response) : NotFound();
    }

    /// <summary>
    /// Creates a new item.
    /// </summary>
    /// <response code="201">Successfully created a new item.</response>
    /// <response code="400">Validation error occurred.</response>
    [HttpPost(ApiRoutes.Items.Create)]
    [ProducesResponseType(typeof(Response<ItemResponse>), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateItemRequest itemRequest)
    {
        var newItemId = Guid.NewGuid();

        var item = new Item
        { 
            Id = newItemId,
            Name = itemRequest.Name,
            UserId = HttpContext.GetUserId()
        };

        var created = await _itemService.CreateItemAsync(item);

        if (!created)
        {
            return BadRequest(new ErrorResponse { Errors = new() { new() { Message = "Unable to create item." } } });
        }

        var locationUri = _uriService.GetItemUri(item.Id.ToString());

        var itemResponse = _mapper.Map<ItemResponse>(item);
        var response = new Response<ItemResponse>(itemResponse);

        return Created(locationUri, response);
    }

    [HttpPut(ApiRoutes.Items.Update)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid itemId, [FromBody] UpdateItemRequest updateItemRequest)
    {
        if (!await _itemService.UserOwnsItemAsync(itemId, HttpContext.GetUserId()))
        {
            return BadRequest(new { error = "You do not own this item." });
        }

        var item = await _itemService.GetItemByIdAsync(itemId);
        item.Name = updateItemRequest.Name;

        var itemResponse = _mapper.Map<ItemResponse>(item);
        var response = new Response<ItemResponse>(itemResponse);

        return await _itemService.UpdateItemAsync(item) ? Ok(response) : NotFound();
    }

    [HttpDelete(ApiRoutes.Items.Delete)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid itemId)
    {
        if (!await _itemService.UserOwnsItemAsync(itemId, HttpContext.GetUserId()))
        {
            return BadRequest(new { error = "You do not own this item." });
        }

        return await _itemService.DeleteItemAsync(itemId) ? NoContent() : NotFound();
    }
}