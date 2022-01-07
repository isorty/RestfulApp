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
public class ItemController : Controller
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet(ApiRoutes.Items.GetAll)]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _itemService.GetItemsAsync());
    }

    [HttpGet(ApiRoutes.Items.Get)]
    public async Task<IActionResult> GetAsync([FromRoute] Guid itemId)
    {
        var item = await _itemService.GetItemByIdAsync(itemId);

        return item is not null ? Ok(item) : NotFound();
    }

    [HttpPost(ApiRoutes.Items.Create)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateItemRequest itemRequest)
    {
        var item = new Item
        { 
            Name = itemRequest.Name,
            UserId = HttpContext.GetUserId()
        };

        await _itemService.CreateItemAsync(item);

        var locationUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}/{ApiRoutes.Items.Get.Replace("{itemId}", item.Id.ToString())}";

        var response = new ItemResponse { Id = item.Id };

        return Created(locationUri, response);
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

        return await _itemService.UpdateItemAsync(item) ? Ok(item) : NotFound();
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