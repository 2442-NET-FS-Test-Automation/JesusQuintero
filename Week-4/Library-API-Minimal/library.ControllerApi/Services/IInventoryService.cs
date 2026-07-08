using Library.ControllerApi.DTOs;
using Library.Data.Entities;

namespace Library.ControllerApi.Services;

public interface IInventoryService
{
    public Task<IReadOnlyList<InventoryItem>> AllAsync();
    public Task<InventoryItem?> BySkuAsync(string sku);
    public Task<InventoryItem> AddAsync(InventoryCreateDto dto);
    public Task<bool> RemoveAsync(string sku);
}