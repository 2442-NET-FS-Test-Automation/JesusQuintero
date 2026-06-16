namespace Library.Domain;

public class ItemNotAvailableExeption : LibraryException
{
    

    public ItemNotAvailableExeption(string title)
            : base($"{title} has no copies available to borrow."){ }
}