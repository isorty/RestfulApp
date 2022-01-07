using Microsoft.EntityFrameworkCore;
using RestfulApp.Data;
using RestfulApp.Domain;

namespace RestfulApp.Services;

public class ItemService : IItemService
{
    private readonly DataContext _dataContext;

    public ItemService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        return await _dataContext.Items.ToListAsync();
    }

    public async Task<Item> GetItemByIdAsync(Guid itemId)
    {
        return await _dataContext.Items.SingleOrDefaultAsync(item => item.Id == itemId);
    }

    public async Task<bool> CreateItemAsync(Item item)
    {
        await _dataContext.Items.AddAsync(item);
        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateItemAsync(Item itemToUpdate)
    {
        if (await GetItemByIdAsync(itemToUpdate.Id) is Item item)
        {
            _dataContext.Items.Update(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        return false;
    }

    public async Task<bool> DeleteItemAsync(Guid itemId)
    {
        var item = await GetItemByIdAsync(itemId);

        if (item is null)
            return false;

        _dataContext.Items.Remove(item);

        return await _dataContext.SaveChangesAsync() > 0;
    }
}