using Library.Data.Entities;

namespace Library.Api.Fulfillment;

public class BurstPlanner
{

    // Method to plan Fulfillment order
    public IReadOnlyList<int> OrderbyPriority(IEnumerable<Order> orders)
    {
        // We could make our custom implementation on this - we won't
        // We can use a PriorityQueue - allows for FIFO processing with priority taken into account
        // First int = OrderId, Second Int = Priority
        // We are going to use lower number - higher priority
        PriorityQueue<int, int> pq = new PriorityQueue<int, int>();

        foreach (Order o in orders)
            // Enqueue each order, if it's Priority is expedited, give it a 0 value, if normal give it 1
            pq.Enqueue(o.Id, o.Priority == Priority.Expedited ? 0 : 1);
        
        // This list will hold everything we want to process already in order to pass to our other methods
        var orderbyPriority = new List<int>();


        // While out PriorityQueue has stuff int it - loop and add those things in the order they exit
        // to our orderedByPriority List - uses out params
        while(pq.TryDequeue(out int id, out _))
        {
            orderbyPriority.Add(id);
        }

        return orderbyPriority;
    }
}