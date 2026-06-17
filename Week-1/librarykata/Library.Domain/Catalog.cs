namespace Library.Domain;

public class Catalog
{
    // Backing out catalog is going to be a list.
    // List<T>: Ordered, grow/shrink dinamically, accesible via index.
    // Your default collection - even above Arrays.

    public readonly List<LibraryItem> _items = new();


    //This method is technically redundant - this class basically just wraps the above list
    // BUT if we wanted to restrict people from Adding or Removing or even accessiong via index
    // from other places in the code, we could wrap not only the list, but its instance methods
    // with our own wrapper methods and make them internal, private, protected, etc.

    // Stack<T>: Last in first out - We will modelt a return cart. The most recently returned an item
    // is re-shelved first.
    // Primary methods - Push(): puts an item at the top of the Stack, Pop(): removes the top most item
    public readonly Stack<LibraryItem> _returnCart = new();

    // Queue<T>: First in first out: modeling a hold queue, customers placing holds on books
    // Primary methods - Enqueue(): join the back of the line, Dequeue(): removed from the front of the line.
    public readonly Queue<string> _holdQueue = new();

    // Reading list
    // LinkedList<T>: cheap inserts/removals anywhere in my list, but NO index access.
    public readonly LinkedList<LibraryItem> _readingList = new();

    // HASHSET<T>: unique values, O(1) lookup. Adding a duplicate silently fails.
    // collection of all authors in my catalog
    private readonly HashSet<string> _authors = new();

    // -- Hashset surface --
    public IReadOnlyCollection<string> Authors => _authors;



    // --- List surface ---
    // Wrapping Add/Remove/index is the whole point of encapsulation: callers state intent,
    // the Catalog decides how. Count was already exposed this way; now the rest is too.
    public int Count => _items.Count;
    public LibraryItem this[int index] => _items[index]; // indexer: read catalog[0] like an array, but read-only
    public void Add(LibraryItem item) => _items.Add(item);
    public bool Remove(LibraryItem item) => _items.Remove(item);

    // --- Stack surface (return cart) ---
    public void DropInReturnCart(LibraryItem item) => _returnCart.Push(item);
    public LibraryItem Reshelve() => _returnCart.Pop();   // most-recently-returned first (LIFO)
    public int CartCount => _returnCart.Count;

    // --- Queue surface (holds line) ---
    public void PlaceHold(string member) => _holdQueue.Enqueue(member);
    public string ServeNextHold() => _holdQueue.Dequeue(); // earliest request first (FIFO)
    public int HoldsWaiting => _holdQueue.Count;

    // --- LinkedList surface (reading list) ---
    public void AddToReadingList(LibraryItem item) => _readingList.AddLast(item);
    public void AddNextUp(LibraryItem item) => _readingList.AddFirst(item); // jump to the front of the list
    // Expose as IEnumerable so callers can foreach over it but cannot mutate the linked list directly.
    public IEnumerable<LibraryItem> ReadingList => _readingList;
}