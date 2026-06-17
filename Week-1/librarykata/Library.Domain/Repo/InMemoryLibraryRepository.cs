// This class will be out actual Library Catalog store of info
using Serilog;

namespace Library.Domain;

public class InMemoryLibraryRepository : ILibraryRepository
{

    // Because we don't have an outside store of info(like a SQL databese)
    // we are kind of forced ti reply on a list. We will store info outside
    // of program execution - I promise

    private readonly Dictionary<int, LibraryItem> _items = new();

    public void Add(LibraryItem item)
    {
        //_items.Add(item);

        // New dictionary add code
        _items.Add(item.Id, item); // If we use this method - it DOES throw when we add a duplicate
        // _items[item.Id] = item; - Alternative addig syntax

        // We just added a new item - thats a significant event - Lets log it.
        // Notice not usig string interpolation - this uses Serilog's template
        // string format
        Log.Information("Added {Title} (id: {id})", item.Title, item.Id);
    }

    public List<LibraryItem> GetAll()
    {
        // Don't want to accidentally pass a pointer to my real list
        // return a new copy of the list
        // return _items.ToList();

        // Instead of refactoring to work with a dictionary for the return
        // we can just ask for a list of all values in the dictionary
        return _items.Values.ToList(); 
    }

    public LibraryItem GetById(int id)
    {
        // In order to find an item in our collection with the given Id
        // we need to search for it. We could use something like LINQ,
        // but that's is own lesson/day
        // foreach(LibraryItem item in _items)
        // {
        //     if(item.Id == id) return item;
        // }



        // New dictionary backed lookup code
        // TryGetValue uses an out parameter. We pass it some value to do key basek lookup
        // We also need to use the out keyword, and give a type and variable name for the second return.
        // ? - means that this might be a null (if we don't find anything)
        if (_items.TryGetValue(id, out LibraryItem? item)) // using an out parameter to get a second return value
        {
            return item;
        }

        // If we make it here - we exited the foreach without finding an item for that id
        Log.Warning("Lookuo failed for id {id}", id);
        throw new ItemNotFoundExeption(id);
        
    }

    public bool Remove(int id)
    {
        // foreach(LibraryItem item in _items)
        // {
        //     if(item.Id == id) 
        //     {
        //         _items.Remove(item);
        //         Log.Information("Removed item with id {Id}", id); // Log the removal
        //         return true;
        //     }
        // }

        if (_items.Remove(id)) // .Remove() - returns a true and removes the item it found, returns false otherwise
        {
            Log.Information("Removed item with id {Id}", id); // Log the removal
            return true;
        }

        Log.Information("Removal failed for item with id {Id}", id);
        return false;
    }
}