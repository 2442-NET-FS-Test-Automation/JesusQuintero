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
    public static void Main()
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

        Log.CloseAndFlush(); // 
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

}