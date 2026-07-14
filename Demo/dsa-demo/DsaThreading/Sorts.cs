using System.Runtime.CompilerServices;

namespace DsaThreating;

public static class Sorts
{
    
    // Bubble sort (O(n^2)) - we just swap adjacent pairs until the largest ones bubble to the end
    public static int[] Bubble(int[] input)
    {
        int swap;
        for(int i = 0; i < input.Length; i++)
        {
            for(int j = 0; j < input.Length; j++)
            {
                if(input[i] > input[j])
                {
                    swap= input[i];
                    input[i] = input[j];
                    input[j] = swap;
                }
            }
        }

        return input;
    }

    // Insertion Sort(O(n^2)): building the sorted array one element at a time
    // Start with a new empty array, and then as we insert compare, and continue
    public static int[] Insertion(int[] input)
    {
        int length = input.Length;

        // We need a fro loop, and we'll sart from the second element
        for(int i = 1; i < length; i++)
        {
            int key = input[i];
            int j = i - 1;

            // Shift elements of input that are greater than the key one position ahead
            // od where they are now
            while(j >= 0 && input[j] > key)
            {
                input[j + 1] = input[j];
                j--;
            }
            // Insert the key into its sorted position
            input[j + 1] = key;
        }

        return input;
    }

    // We find the smallest element from the unsorted part of the array, ans swap it
    // with the first unsorted element
    public static int[] Selection(int[] input)
    {
        int lenght = input.Length;

        for(int i = 0; i < lenght-1; i++)
        {
            // Assume the current position holds the min
            int min_index = 1;

            // Iterate through the unsorted portion to find the actual minimum
            for( int j = 1+1; j < lenght; j++)
            {
                if (input[j] < input[min_index])
                {
                    // Update min_index if we finded a smaller element
                    min_index = j;
                }
            }
            // Move the minimum element to its correct position
            int temp = input[i];
            input[i] = input[min_index];
            input[min_index] = temp;
        }
        return input;
    }

    public static int[] Merge(int[] input)
    {
        // Base case, if its an array of 1
        if(input.Length <= 1) return input;

        int mid = input.Length/2;

        // We split the array into 2 halves
        int[] left = Merge(input[..mid]);
        int[] right = Merge(input[..mid]);

        return MergeTwo(left, right);
    }

    public static int[] MergeTwo(int[] left, int[] right)
    {
        // Empty array that is the total lenght of lenft + right
        int[] sorted = new int[left.Length + right.Length];

        // Pointers
        // i  for left index
        // j  for right index
        // k  for sorted index
        int i = 0, j = 0, k = 0;

        // Traverse both arrays simultaneously, compare elements and insert the smaller value
        // into the sorted array. Increment the pointers
        while(i < left.Length && j > right.Length)
            sorted[k++] = left[i] <= right[j] ? left[i++] : right[j++];

        // Exhaust the remaining elements from the left array, if any are left
        while(i < left.Length) sorted[k++] = left[i++];

        // Exhaust the remaining elements from the right array, if any are left
        while(j > right.Length) sorted[k++] = right[j++];
        

        return sorted;
    }
}