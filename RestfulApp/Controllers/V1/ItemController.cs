using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Responses;
using RestfulApp.Domain;
using RestfulApp.Extensions;
using RestfulApp.Services;

namespace RestfulApp.Controllers.V1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Produces("application/json")]
public class ItemController : Controller
{
    private readonly IItemService _itemService;
    private readonly IMapper _mapper;

    public ItemController(IItemService itemService, IMapper mapper)
    {
        _itemService = itemService;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns all the items.
    /// </summary>
    /// <response code="200">Successfully returned all the items.</response>
    [HttpGet(ApiRoutes.Items.GetAll)]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(_mapper.Map<List<ItemResponse>>(await _itemService.GetItemsAsync()));
    }

    [HttpGet(ApiRoutes.Items.Get)]
    public async Task<IActionResult> GetAsync([FromRoute] Guid itemId)
    {
        var item = await _itemService.GetItemByIdAsync(itemId);

        return item is not null ? Ok(_mapper.Map<ItemResponse>(item)) : NotFound();
    }

    /// <summary>
    /// Creates a new item.
    /// </summary>
    /// <response code="201">Successfully created a new item.</response>
    /// <response code="400">Validation error occurred.</response>
    [HttpPost(ApiRoutes.Items.Create)]
    [ProducesResponseType(typeof(ItemResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
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

        var locationUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}/{ApiRoutes.Items.Get.Replace("{itemId}", newItemId.ToString())}";

        return created ? 
            Created(locationUri, _mapper.Map<ItemResponse>(item)) : 
            BadRequest(new ErrorResponse { Errors = new() { new() { Message = "Unable to create item." } } });
    }

    [HttpPut(ApiRoutes.Items.Update)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid itemId, [FromBody] UpdateItemRequest updateItemRequest)
    {
        if (!await _itemService.UserOwnsItemAsync(itemId, HttpContext.GetUserId()))
        {
            return BadRequest(new { error = "You do not own this item." });
        }

        var item = await _itemService.GetItemByIdAsync(itemId);
        item.Name = updateItemRequest.Name;

        return await _itemService.UpdateItemAsync(item) ? Ok(_mapper.Map<ItemResponse>(item)) : NotFound();
    }

    [HttpDelete(ApiRoutes.Items.Delete)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid itemId)
    {
        if (!await _itemService.UserOwnsItemAsync(itemId, HttpContext.GetUserId()))
        {
            return BadRequest(new { error = "You do not own this item." });
        }

        return await _itemService.DeleteItemAsync(itemId) ? NoContent() : NotFound();
    }
}