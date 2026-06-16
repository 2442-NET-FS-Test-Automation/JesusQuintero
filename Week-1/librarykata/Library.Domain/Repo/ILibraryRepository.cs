namespace Library.Domain;

public interface ILibraryRepository
{
    // This interface is an abstraction over an actual repository class (concrete implementation)
    // Lets think of things we want to be able to do against our Library's store of information

    // At minimum we probably want to provide for basic CRUD

    // Create new items in my library
    void Add(LibraryItem item); // Takes in the item to be added, can be anything that inherits from the parent

    // Read / Get library items
    LibraryItem GetById(int id); // Trows ItemNotFoundException if the item doesn't exist at all
    List<LibraryItem> GetAll();

    // Update library items

    // Delete items in my library
    bool Remove(int id); // Takes an item Id of item to delete
}