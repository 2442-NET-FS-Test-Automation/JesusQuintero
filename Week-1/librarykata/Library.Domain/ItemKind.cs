// This is going to be an Enum
// Its a custom value type, where basically enumerate possible values ahead of time

namespace Library.Domain;

public enum ItemKind
{
    // My enum definition contains posible values for an instance of this enum.
    // An ItenKind enum can ONLY ever be one of these 3 things. I can come back and add more later.

    Book,
    RerefenceBook,
    Magazine
}