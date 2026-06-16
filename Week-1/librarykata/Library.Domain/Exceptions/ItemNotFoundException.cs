namespace Library.Domain;

public class ItemNotFoundExeption : LibraryException
{
    // We can hold the offendinf Id that triggered the exception
    // we will use this for logging later
    public int Id {get; }

    public ItemNotFoundExeption(int id) 
            : base($"No library item withi id {id}")
    {
        Id = id;
    }
}