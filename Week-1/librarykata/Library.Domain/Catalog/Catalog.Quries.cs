using System.Collections;

namespace Library.Domain;


// The second half of my class
// I don't have to mirror the interface implementation or any inheritance across both class files
// however, I can still only inherit from parent
public partial class Catalog : IEnumerable<LibraryItem>
{
    // this is the one that we actually want to provide logic for, the one that uses a generic
    public IEnumerator<LibraryItem> GetEnumerator()
    {
        foreach( LibraryItem item in _items)
        {
            // We wanto to lazily returns items one a time, we don't want to return a second list
            // or anything like that. We will use "yield" with out return
            yield return item;
        }
    }


    // This version (non-generic version) is ODL - kept it the IEnumerable for backwards compatibility reasons.
    // What we are doing is simply routing it to the IEnumerator<LibraryItem> GetEnumerator() method
    IEnumerator IEnumerable.GetEnumerator()
    {
        // returns a call to IEnumerator<LibraryItem> GetEnumerator()
        return GetEnumerator();
    }

    // Lets make a method to return only lendable items (things that implemet ILendable)
    public IEnumerable<LibraryItem> Lendable()
    {
        
        foreach (LibraryItem item in _items)
        {
            // Checking for type via "is"
            if (item is ILendable)
            {
                yield return item;
            }
        }
    }

    // Search function for the Catalog
    // We are going to use Predicate to pass a delegate to our function
    // A delegate is just a reference to method in an argument list
    // Predicate<LibraryItem> match represents a function that takeas a LibraryItem, and returns a boolean

    // When we call this Find(), we will combine it with Lambda. Lambda's are the C# implementation
    // of anonymous of arrow functions. Just a quick definitiom that we don;t bother storing a reference to.
    // authorItems = Find(item => item.Author == "Frank Herbert"); - "find every item were it;s author equals "Frank Herbert'
    public List<LibraryItem> Find(Predicate<LibraryItem> match)
    {
        // match is a method, not an object or a value
        // its a pointer to some method that gets passed in when call Find()
        List<LibraryItem> foundItems = new();

        foreach (LibraryItem item in _items)
        {
            if (match(item))
            {
                foundItems.Add(item);
            }
        }
        return foundItems;
    }
}