namespace Library.Data.Entities;

public class InventoryItem
{
    public int Id {get; set; }
    public int ProductId {get; set; } // FK - 1:1 product
    public Product Product {get; set; } = default!; // We can EF give a default value
    public int CurrentStock {get; set; } // how many of this do we have

    // Adding a RowVersion preoperty - we will use this in OnModelCreating
    // We will use this to track concurrency
    public byte[] RowVersion {get; set; } = default!;

}