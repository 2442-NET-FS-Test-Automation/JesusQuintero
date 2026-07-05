namespace DsaThreating;

public static class Searches
{
    // Linear search: O(n) - walk the array until we find what we want
    // Sorted or unsorted doesn't really matter, unsorted OK
    public static int LinearSearch(int[] data, int target)
    {
        // We could probably use a foreach but that is itself an abstraction
        for(int i = 0; i < data.Length; i++)
        {
            if(data[i] == target) return i;
        }
        // if we don't fin it return -1
        return -1;
    }

    // Binary Search - halve the search space each step
    // O(log n) - but we must be sorted before we start
    public static int BinarySearch(int[] sorted, int target)
    {
        int i = sorted.Length/2, pivot = 0, size = sorted.Length/2;

        while(size > 0)
        {
            size = size / 2;
            if(sorted[i + pivot] < target)
            {
                pivot = i;
                i = i + size;
                
            }
            else if(sorted[i + pivot] > target)  i = size;
            else return i + pivot;
        }
        return -1;
    }

}