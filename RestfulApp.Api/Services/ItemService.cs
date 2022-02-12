using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestfulApp.Api.Data;
using RestfulApp.Api.Data.Models;
using RestfulApp.Api.Domain;

namespace RestfulApp.Api.Services;

public class ItemService : IItemService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public ItemService(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<List<Item>> GetItemsAsync(GetAllItemsFilter filter = null, PaginationFilter paginationFilter = null)
    {
        var queryable = _dataContext.Items.AsQueryable();

        queryable = AddFiltersOnQuery(filter, queryable);

        if (paginationFilter is null)
        {
            return _mapper.Map<List<Item>>(queryable.ToListAsync());
        }

        var skipCount = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

        var itemDtos = await queryable.Skip(skipCount)
                                      .Take(paginationFilter.PageSize)
                                      .ToListAsync();

        var items = _mapper.Map<List<Item>>(itemDtos);

        return items;
    }

    public async Task<Item> GetItemByIdAsync(Guid itemId)
    {
        var itemDto = await _dataContext.Items
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == itemId);

        return _mapper.Map<Item>(itemDto);
    }

    public async Task<bool> CreateItemAsync(Item item)
    {
        var itemDto = _mapper.Map<ItemDto>(item);

        await _dataContext.Items.AddAsync(itemDto);

        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<Item> UpdateItemAsync(Item itemToUpdate)
    {
        if (await _dataContext.Items.AsNoTracking().SingleOrDefaultAsync(item => item.Id == itemToUpdate.Id.Value) is ItemDto itemDto)
        {
            itemDto = _mapper.Map(itemToUpdate, itemDto);
            _dataContext.Items.Update(itemDto);
            return await _dataContext.SaveChangesAsync() > 0 ? _mapper.Map<Item>(itemDto) : null;
        }

        return null;
    }

    public async Task<bool> DeleteItemAsync(Guid itemId)
    {
        if (await _dataContext.Items.SingleOrDefaultAsync(item => item.Id == itemId) is ItemDto itemDto)
        {
            _dataContext.Items.Remove(itemDto);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        return false;
    }

    public async Task<bool> UserOwnsItemAsync(Guid itemId, string userId)
    {
        return await _dataContext.Items
            .AsNoTracking()
            .AnyAsync(item =>
                item.Id == itemId &&
                (string.Equals(item.UserId, userId) || string.IsNullOrEmpty(item.UserId)));
    }

    private static IQueryable<ItemDto> AddFiltersOnQuery(GetAllItemsFilter filter, IQueryable<ItemDto> queryable)
    {
        if (!string.IsNullOrEmpty(filter?.Name))
        {
            queryable = queryable.Where(item => item.Name.ToLower().Contains(filter.Name.ToLower()));
        }

        return queryable;
    }
}