// Lets actually start modeling stuff

namespace Library.Domain;

public class OldBook
{
    // Things about a boo we can model - what is the "shape" of a 
    // Because I want to use no-arg Constructto, its best practice to make
    // my properties nullable
    public string? Title { get; private set; } // auto property syntax - no writing getters and setters
    public string? Author { get; private set; }
    public int? CopiesAvailable { get; private set; }

    // The same way we can have static methods (belog to the class)
    // we can have static properties/members
    private static int _nextId = 1; // By convention, static properties have an underscore

    public int Id { get;} // No setter, I don't want someone to reassign this

    // Every calss has a very specific method within it
    // The constructor - you can have as many as you need/want
    // Lets make a full argument constructor
    public OldBook(string title, string author, int copiesAvailable)
    {
        Id = _nextId++; // Get the value of _nextId, assign it, increment it
        Title = title;
        Author = author;
        CopiesAvailable = copiesAvailable;
    }

    public OldBook(){}

    // Our first instance method - no "static" keyword, just
    // an access modifier + return type + any arguments if any
    public bool Checkout()
    {
        // Attemp to ckeckout a book - if copies is already 0, return false
        if (CopiesAvailable == 0)
        {
            return false;
        }

        // Otherwise, we pass over the above code block
        // We can decrement the aviable copies and return true
        CopiesAvailable--;
        return true;
    }

    // Providing for return behavoir
    public void Return() => CopiesAvailable++;

    // Overriding a toString
    public override string ToString()
    {
        // Commented out below is a call to base.ToString()
        // We can use the base keyword to refer to the parent class of the class we are working in
        // Book's parent is object, so this is calling the default ToString()
        // return base.ToString();

        return $"Title: {Title} by {Author}: {CopiesAvailable} available for checkout";
    }

}
