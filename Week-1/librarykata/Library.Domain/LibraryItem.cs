namespace Library.Domain;

// Library item will be an abstract class - it cannot be instantiated
// it WILL still have a constructos - because child classes NEED to be able
// to call their parent's constructor - but WE can't call it via new
public abstract class LibraryItem
{
    // Things about a boo we can model - what is the "shape" of a 
    // Because I want to use no-arg Constructto, its best practice to make
    // my properties nullable
    public string? Title { get; private set; } // auto property syntax - no writing getters and setters
    public string? Author { get; private set; }

    // The same way we can have static methods (belog to the class)
    // we can have static properties/members
    private static int _nextId = 1; // By convention, static properties have an underscore

    public int Id { get;} // No setter, I don't want someone to reassign this


    // My abstract class DOES have a constructor
    // So far, we've dealt with public and private access modifiers.
    // Publuc: anyone can see/call this
    // Private: only accesible within this class
    // Protected: this class derived (child) classes only
    protected LibraryItem(string title, string author)
    {
        Id = _nextId++;
        Title = title;
        Author = author;
    }

    // Abstract method - only a signature - no body
    public abstract string Describe();

    // Abstract classes CAN contain concrete implemetations - and we can mix our abstract methods to save time later
    // potencialy. Our child classes WILL imprement Describe() - use that dor the ToString()
    public override string ToString() => Describe();

    // Concrete methods have a body, Abstract methods MUST be overriden... virtual methods have a body and MAY be overriden
    public virtual string ShelfLabel()
    {
        return $"{Id}: {Title}";
    }
}