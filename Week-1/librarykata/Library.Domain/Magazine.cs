namespace Library.Domain;

// Sealed is pretty simple, it measn this class is not inheritable
// Nobody can be a child of Magazine. More a signal of intent and design than anything, but still useful.
public sealed class Magazine : LibraryItem, ILendable
{
    public int CirculationCopies { get; private set; }
    public string Publisher {get; private set;}

    public Magazine(string title, string author, int circulationCopies, string publisher) 
        : base(title, author)
    {
        CirculationCopies = circulationCopies;
        Publisher = publisher;
    }

    public override string Describe()
    {
        return $"{Title} magazine, published by {Publisher}";
    }

    // Providing implementation via new instead of override - has implications for later
    // This is technically Method Hidinf - depends on the reference type
    // Calling this method in an objectinstatiate like this:
    // LibraryIten sportsIllustrated = new Magazine(...); - calls LibraryItems's ShelfLabel
    // This is most likely not what you want.
    // new vs override - very different behavior

    public new string ShelfLabel()
    {
        return $"MAG-{Id} {Title}";
    }

    public bool Checkout()
    {
        // Attemp to ckeckout a book - if copies is already 0, return false
        if (CirculationCopies == 0)
        {
            return false;
        }

        // Otherwise, we pass over the above code block
        // We can decrement the aviable copies and return true
        CirculationCopies--;
        return true;
    }

    // Providing for return behavoir
    public void Return() => CirculationCopies++;
}