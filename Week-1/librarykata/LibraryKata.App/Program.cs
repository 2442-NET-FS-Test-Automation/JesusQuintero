// If i have code from another namespace I wanto to use here - I use a using statement
using System.Runtime.CompilerServices;
using Library.Domain;
using Serilog;
namespace LibraryKata.App; // A namespace is like a bucket or logical container for different
// related code files.
public class Program
{
    
    // Now we are moving away from the Python file style Top-Level statements
    // So we need a class to hold our Main() method. The previous style with no class
    // or main - implicity had a Main() under the hood. 

    // public - accessible across the program
    // static - Main can be called upon without a Program object. It is a Static/class method. 
    // void - it doesn't return anything
    public static async Task Main()
    {   
        // Lets configure Serilog here before any code execution
        // Serilog works via a singleton object. Its shared globally
        // thoughout the app, configure once use anywhere.
        Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information() // VERBOSE > DEBUG > INFO > WARNING > Error > Fatal
                    .WriteTo.Console()          // Sink: where do my logs go? text file, database, etc?
                    .CreateLogger();            // Create logger based on the configuration above




        // When I call dotnet run, it finds Main() and begins code execution at the first line of the 
        // main method. I wrote my code, inside DataTypesAndOperators() - a separate method. So if I want 
        // that code to run, I need to call it inside Main()
        Program.DataTypesAndOperators();
        Console.WriteLine("\n\n");
        Program.ClassesExample();
        Console.WriteLine("\n\n");
        Program.OopDemo();

        Console.WriteLine("\n\n");
        Program.CollectionsDemo();

        Console.WriteLine("\n\n");
        Program.ExceptionsDemo();

        Console.WriteLine("\n\n");
        Program.AdvancedClassesDemo();

        Console.WriteLine("\n\n");
        await Program.asyncHttpDemo();


        // In case thera are any lingering logs by the time we hit line 41 above
        // Don't just stop execution, write the logs to their sink THEN close the program
        Log.CloseAndFlush(); 
    }

    

    // private - accessible only within this class
    // static - it belongs to the class, not objects of the class
    // void - returns nothing
    private static void DataTypesAndOperators() // If I had arguments, or inputs for this method,
    { // they would go inside the parenthesis after the method name 
        Console.WriteLine("=== Data types and operators ==");

        // C# is a Strongly typed language
        // We cannot just create variables and shove whatever we want into them like JS or Python
        int copies = 3; // whole numbers
        double lateFee = 1; // floating point numbers (decimals)
        bool isMember = true; // true/false values
        char shelf = 'A'; // single character
        string title = "Clean Code"; // text, strings are reference types

        // Operators 
        string user = "Jon"; // Single = is the assignment operator. 
        int total = copies * 2; // example of an arithmetic operator, like + - * / 
        bool isEnough = total > 4; // comparison - This line compares the value in total to 4, if it is greater
        // than 4, isEnough will get 'true', otherwise it will get 'false'
        // >, <, >=, <= - comparison operators
        bool exactlySix = total == 6; // equality. Single equals is assignment, double equals is equality.
        // unlike JS there is NO === all equality in C# is Strict equality
        bool lendable = isMember && isEnough; //logical operators
        // && - and, || - or, ! - reverses the condition that follows, ^ logical XOR - returns true if ONLY one condition is true

        // This is the basic way to construct strings from other strings
        // String concat - it works! But it can be messy
        Console.WriteLine(title + " has been checked out by " + user);

        // We can create much cleaner formatted strings
        // using String Interpolation - a string with a $ before the opening quote
        Console.WriteLine($"{title} on shelf {shelf}: {copies} copies, fee {lateFee}"); 

        // C# has ALOT of shorthands and little shortcuts that you can find and use 
        // to make your code easier to write. For example, lets say I want to add 1 to the value of total
        // I could do something like
        // total = total + 1; - ORRR
        total += 1; // arithmetic shorthand for the same thing, also works for *= /= -=

    }

    private static void ControlFlow()
    {
        Console.WriteLine("/n=== Control Flow ===");

        // IF - ELSE IF - ELSE
        int copiesAvailable = 0;
        //bool isMember = true;
        if (copiesAvailable > 1)
        {
            Console.WriteLine("Many avilable for checkout");
        }
        else if (copiesAvailable == 1)
        {
            Console.WriteLine("Last copy!");    
        }
        else
        {
            Console.WriteLine("Out of stock");
        }

        //SWITCH

        string genre = "Mistery";

        // Classic switch - notice C# cares about intent a lot! No fall through like in other languages
        switch (genre)
        {
            case "Mistery":
                Console.WriteLine("Check section A!");
                break;
            case "Science-Fiction":
                Console.WriteLine("Check section F!");
                break;
            default:    // While optional, a default case to catch any edge cases is best practice
                Console.WriteLine("Uh Oh");
                break;
        }

        // New in .NET 8, Switch Expressions! You don't have to use these - they probably won't come up in QC
        //but they're used out in real world code, so here is an example. In a switch expression, we want
        // a return value from the switck - se can then use that value to print out a result

        string section = genre switch
        {
            // This is my expression body
            "Mistery" => "Section A",
            "Science-Fiction" => "Section F",
            _ => "Uh Oh"    // Default
        };
        Console.WriteLine(section);

    }

    private static void Loops()
    {
        // C# provides for loops as well, same as Java and any other Language
        // For, While, Do-While, etc

        for(int day = 1; day <= 3; day++)
        {
            Console.WriteLine($"Reminder day {day}: fee so far{CalculateLateFee(day)}");
        }

        int onShelf = 3;
        while(onShelf > 0)
        {
            Console.WriteLine($"{onShelf} copies on the shelf!");
            onShelf--; // quick decrement shorthand
        }

        Console.WriteLine("No copies on shelf!");


        string myString = "Dog";

        myString = "Cat";
    }

    // I can use this shorthand for one line methods
    private static decimal CalculateLateFee(int daysLate) => daysLate * 2; 

    private static void ArraysWork()
    {
        // C# provides for Arrays as well as lists and other collections - we'll get to those later.
        string[] books = {"Dune", "Harry Potter", "Percy Jackson", "The lord of the Rings"};

        Console.WriteLine(books[2]); // I can access individual elements - keeping in mind we index at 0

        // C# allows for for-each loops
        foreach(string book in books)
        {
            Console.WriteLine(book);
        }
    }


    private static void ClassesExample()
    {
        Console.WriteLine("Using our domain Book class");

        // Instantiating my first book, calling the constructor via "new" keyword
        Book dune = new Book("Dune", "Frank Herbert", 3);
        Book littlePrince = new Book("The Little Prince", "Antoine de Saint-Exupéry", 0);


        // If I want to print book info, I can just pass the book variable
        // It calls the toString() for me. The next two lines do the same thing
        Console.WriteLine(dune);
        Console.WriteLine(littlePrince.ToString());

        Console.WriteLine($"Checking out Dune: {dune.Checkout()}"); // True
        Console.WriteLine($"Checking out The Little Prince: {littlePrince.Checkout()}"); // False
    }


    public static void OopDemo()
    {
        Console.WriteLine("\n\n == OOP Demo stuff ==");

        // Leverageing polymorphism - Books, ReferenceBooks, Magazines, all are LibraryItems
        LibraryItem[] catalog =
        {
            new Book("Dune", "Frank Herbert", 2),
            new ReferenceBook("C# Language Standarts", "Microdoft", "Technology"),
            new Magazine("Sports Illustrated", "Francisco Something", 5, "Conde Naste")
        };

        foreach(LibraryItem item in catalog)
        {
            Console.WriteLine(item.Describe());
        }

        // We can enven use interfaces as reference types
        foreach(LibraryItem item in catalog)
        {
            if(item is ILendable lendable)
            {
                Console.WriteLine($"{item.Title}: checkout -> {lendable.Checkout()}");
            }
            else
            {
                Console.WriteLine($"{item.Title} is Reference Only.");
            }

        }

        // Override vs new behavior
        Magazine wired = new Magazine("Wired", "Luis", 3, "Conde Nast");
        LibraryItem baseMag = wired;

        Console.WriteLine("\n\n== Override vs New on the same object, different ref type");
        Console.WriteLine($"Magazine reference -> {wired.ShelfLabel()}");
        Console.WriteLine($"LibraryItem reference -> {baseMag.ShelfLabel()}");
    }


    // Collections demo stuff
    private static void CollectionsDemo()
    {
        Console.WriteLine("===== Collection's Demo Stuff =====");

        // Creating a catalog object
        // Because this is backed by a list, it grows and shrinks for us

        Catalog catalog = new();

        // I could create my objects
        Book dune = new Book("Dune", "Frank Herbert", 3);
        
        // Then add them
        catalog._items.Add(dune);

        // I can also just call a constuctor inside the Add() method call
        // Methods having their arguments satisfied by the return of other methods is a common pattern
        // and sometimes you'll get like 4-5 callbacks deel in tools like AP.NET
        catalog._items.Add(new ReferenceBook("C# Language", "Microsoft", "Technology"));
        catalog._items.Add(new Magazine("Nat Geo", "Charlie", 4, "Conde Nate"));

        Console.WriteLine($"Calatog holds {catalog._items.Count}; first is {catalog._items[0]}");

        // Enum + Struct use
        ItemKind kind = ItemKind.Magazine; // Example of selecting an enum value
        ShelfLocation location = new ShelfLocation(3, 12); // Struct - looks a lot like a class, but is a VALUE type

        Console.WriteLine($"{kind} sits at {location}");

        Book duneCopy = dune; // Copies the reference
        // Lets say I modify duneCopy, what happens to the data in dune?
        // all we copued was the pointer - these two things are not independent

        ShelfLocation location2 = location; // Copies the data/fuelds
        // theses are not linked in the same way, I can edit the data in one without touching the other

        // Generics: our own Shelf<T> that can hold anything - though technically all the collections
        // we used thusfar have been generic classes themselves
        Shelf<LibraryItem> shelf = new Shelf<LibraryItem>(2);
        Shelf<int> intShelf =  new Shelf<int>(200);

        shelf.TryAdd(catalog._items[0]);
        shelf.TryAdd(catalog._items[1]);

        Console.WriteLine($"Trying to add a third thing in our catalog: {shelf.TryAdd(catalog._items[2])}");

    }

    public static void ExceptionsDemo()
    {
        Console.WriteLine("\n == Exceptions, patterns, logging ==");

        // By usimg Liskov Substitution from S.O.L.I.D., if I later swap to
        // a SQLLibraryRepo or whatever, this is the only line I haave to change
        ILibraryRepository repo = new InMemoryLibraryRepository();

        // Injecting our existing repo object to statisfy LibraryUnitOfWork's dependency
        IUnitOfWork libraryWork = new LibraryUnitOfWork(repo);

        // Create a book, but using our factory method
        LibraryItem dune =  LibraryItemFactory.Create(ItemKind.Book, "Dune", "Frank Herbert", copies : 3);

        repo.Add(dune);

        // Magazines need a publisherm but we peovided a default value for the publisher argument in Create
        // lets see if it works
        repo.Add(LibraryItemFactory.Create(ItemKind.Magazine, "Wired", "Axel Xman", copies:2));

        // Pretend we're commiting changes to a DB or something
        libraryWork.Stage("Added 2 items");
        libraryWork.Commit();

        // We went through the trouble of creating custom exceptions
        // Lets actually see them work for us. If you have code that can potentially fail
        // wrap it in a try-catch (optional finally)
        try
        {
            // Potentially offending code goes here
            LibraryItem missing = repo.GetById(99);
            Console.WriteLine(missing.Describe()); // we won't hit this I believe
        }
        catch (ItemNotFoundExeption ex)
        {
            // We stored the offending id on the exception itself, here we can ask fot it for logging
            Log.Error("Lookout failed for id {Id}: {Message}", ex.Id, ex.Message);
        }
        catch (LibraryException ex)
        {
            Log.Error("Library Error: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error("Non Library Error: {Message}", ex.Message);
        }
        finally // Optional but adding a finally block adds code that runs
        {       // Whether an exception is caught or not
            // Code in a finally block will run even if the try ends in a return
            // Useful for DB operations where you want to cleanup but you found
            // the object to return
            Console.WriteLine("Hit out finally block - lookup attemp done");
        }

        Book noCopies = new Book("Count of Montecristo", "Alejandro Dumas", 0);

        try
        {
            Borrow(noCopies);
        }
        catch (ItemNotAvailableExeption ex)
        {
            Log.Warning("Borrow refused: {Message}", ex.Message);
        }
    }

    public static void Borrow(Book book)
    {
        // We can use the Checkout (boolean return) method from the book object
        // in an if or something
        if (!book.Checkout())
        {
            throw new ItemNotAvailableExeption(book.Title);
        }
    }

    public static void AdvancedClassesDemo()
    {
        Console.WriteLine("\n == Advanced Classes ==");

        // First, a quick detour, lets interact with the garbage collector
        Console.WriteLine(GC.GetTotalMemory(forceFullCollection : false) / 1024);

        ILibraryRepository repo = new InMemoryLibraryRepository();

        LibraryItem dune =  LibraryItemFactory.Create(ItemKind.Book, "Dune", "Frank Herbert", copies : 3);

        repo.Add(dune);
        repo.Add(LibraryItemFactory.Create(ItemKind.Magazine, "Wired", "Axel Xman", copies:2));
        repo.Add(LibraryItemFactory.Create(ItemKind.Book, "Dune Messiah", "Frank Herbert", copies:3));
        repo.Add(LibraryItemFactory.Create(ItemKind.RerefenceBook, "C# Language Reference", "Microsoft", 1, section :"Technology"));

        Catalog catalog = new();

        foreach(LibraryItem item in repo.GetAll())
        {
            catalog.Add(item);
        }


        Console.WriteLine($"We have {catalog.Authors.Count} unique authors in our catalog");

        foreach (string author in catalog.Authors)
        {
            Console.WriteLine(author);
        }

        // Lets search our catalog now that it's backed by a dictionary
        // Lets use our Find() method
        List<LibraryItem> byFrankHerbert = catalog.Find(item => item.Author == "Frank Herbert");
        Console.WriteLine($"There are {byFrankHerbert.Count} books by Frank Herbert");

        // Lets see how many items in the catalog are Lendable
        Console.WriteLine("We have a mix of lendable and non-lendable items");
        foreach(LibraryItem item in catalog.Lendable())
        {
            Console.WriteLine($"{item.Title}");
        }
    }

    public static async Task asyncHttpDemo()
    {
        // We wrote our client object so lets use it
        OpenLibraryClient client = new();

        // Array to hold some Isbn's
        string[] isbns = {"9780132350884", "9780201633610"};

        // I want to fetch the data from OpenLibrary for both ISBNs
        // I do not want to sit here and type the same code calling the same method for both ISBNs
        // I would end up awating two almost identical calls - thats valid but the curricula says "optimizing async code"
        Task<LibraryItem?>[] fetchedBooks = new Task<LibraryItem?>[isbns.Length];

        // Next, we loop through the array and call FetchByIsbnAsync - we use a traditional C-syntax for-loop
        // because we care about indexes for this
        for(int i = 0;i < isbns.Length; i++)
        {
            // Notice, tihis is an async method call - but we didn't await it.
            fetchedBooks[i] = client.FetchByIsbnAsync(isbns[i]);
        }

        // If we ONLY wanted one book, and we just had one isbnm we could do something like the following.
        // foundBook = await client.FetchByIsbnAsync("1234567890123"); 
        LibraryItem?[] foundBooks = await Task.WhenAll(fetchedBooks);

        // This works, bu what if there's nothing there
        // LibraryItem? firstBookFound = foundBooks[0];

        // To be safe we can use a quick ternary operator. Like a quick if-else check
        // ternary syntax (some condition to check) ? trueValue : falseValue
        LibraryItem? firstBookFound = foundBooks.Length > 0 ? foundBooks[0] : null;

        // Using WhenAll to do current fetching. If we didn't do this, and we awaited EVERY SINGLE call one by one
        // Think about the amount of latency we'd be eating.

        Console.WriteLine($"Fetched: {firstBookFound?.Describe() ?? "Nothing"}");

        // Boxing and Unboxing - mostly deprecated, replaced by Generics
        // Sometimes we needed to store value types on the heap, think of adding an int to a List. Before Generics (List<int>)
        // we had arrayList to acomplish the same thing. Under the hood, an ArrayList couldn't accept value types
        int toBeBoxed = 6;
        
        // We "box" it, by giving wrapping it in an object reference
        // So now it's on the heap
        object boxed = toBeBoxed; // This boxing process is something like 15-20x slower than just assign an int

        // Later, say, when we read something from the ArrayList into an int variable
        int unboxed = (int)boxed;

        // How we can avoid this
        // DON'T USE NON GENERIC COLLECTIONS
        // List<T> is modern, uses generics, avoid boxed-unboxed
        // ArrayList - deprecated, slow, uses boxing and unboxing

    }
}