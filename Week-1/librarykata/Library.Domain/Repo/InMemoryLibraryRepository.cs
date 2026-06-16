// This class will be out actual Library Catalog store of info
using Serilog;

namespace Library.Domain;

public class InMemoryLibraryRepository : ILibraryRepository
{

    // Because we don't have an outside store of info(like a SQL databese)
    // we are kind of forced ti reply on a list. We will store info outside
    // of program execution - I promise

    private readonly List<LibraryItem> _items = new();

    public void Add(LibraryItem item)
    {
        _items.Add(item);
        // We just added a new item - thats a significant event - Lets log it.
        // Notice not usig string interpolation - this uses Serilog's template
        // string format
        Log.Information("Added {Title} (id: {id})", item.Title, item.Id);
    }

    public List<LibraryItem> GetAll()
    {
        // Don't want to accidentally pass a pointer to my real list
        // return a new copy of the list
        return _items.ToList();
    }

    public LibraryItem GetById(int id)
    {
        // In order to find an item in our collection with the given Id
        // we need to search for it. We could use something like LINQ,
        // but that's is own lesson/day
        foreach(LibraryItem item in _items)
        {
            if(item.Id == id) return item;
        }

        // If we make it here - we exited the foreach without finding an item for that id
        Log.Warning("Lookuo failed for id {id}", id);
        throw new ItemNotFoundExeption(id);
        
    }

    public bool Remove(int id)
    {
        foreach(LibraryItem item in _items)
        {
            if(item.Id == id) 
            {
                _items.Remove(item);
                Log.Information("Removed item with id {Id}", id);
                return true;
            }
        }

        Log.Information("Removal failed for item with id {Id}", id);
        return false;
    }
}