using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestfulApp.Data;
using RestfulApp.Data.Models;
using RestfulApp.Domain;

namespace RestfulApp.Services;

public class ItemService : IItemService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public ItemService(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<List<Item>> GetItemsAsync(PaginationFilter paginationFilter = null)
    {
        if (paginationFilter is null)
        {
            return _mapper.Map<List<Item>>(await _dataContext.Items.ToListAsync());
        }            

        var skipCount = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

        var itemDtos = await _dataContext.Items
            .Skip(skipCount)
            .Take(paginationFilter.PageSize)
            .ToListAsync();

        var items = _mapper.Map<List<Item>>(itemDtos);

        return items;
    }

    public async Task<Item> GetItemByIdAsync(Guid itemId)
    {
        var itemDto = await _dataContext.Items.SingleOrDefaultAsync(item => item.Id == itemId);

        return _mapper.Map<Item>(itemDto);
    }

    public async Task<bool> CreateItemAsync(Item item)
    {
        var itemDto = _mapper.Map<ItemDto>(item);

        await _dataContext.Items.AddAsync(itemDto);

        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateItemAsync(Item itemToUpdate)
    {
        if (await GetItemByIdAsync(itemToUpdate.Id) is Item item)
        {
            var itemDto = _mapper.Map<ItemDto>(item);
            _dataContext.Items.Update(itemDto);

            return await _dataContext.SaveChangesAsync() > 0;
        }

        return false;
    }

    public async Task<bool> DeleteItemAsync(Guid itemId)
    {
        var item = await GetItemByIdAsync(itemId);

        if (item is null)
        {
            return false;
        }

        var itemDto = _mapper.Map<ItemDto>(item);

        _dataContext.Items.Remove(itemDto);

        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UserOwnsItemAsync(Guid itemId, string userId)
    {
        return await _dataContext.Items
            .AsNoTracking()
            .AnyAsync(item => 
                item.Id == itemId && 
                (string.Equals(item.UserId, userId) || string.IsNullOrEmpty(item.UserId)));
    }
}