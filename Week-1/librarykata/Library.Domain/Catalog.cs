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
    public int Count => _items.Count();

}