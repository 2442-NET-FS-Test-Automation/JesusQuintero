using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DsaThreating;

Console.WriteLine("Hello, World!");

ThreadingDemo();




static async Task ThreadingDemo()
{
    // Lets take a look at how C# manages Threads (OS Threads not CPU threads)
    // In C# Threads are an object - like everything else. Typically they're managed
    // by the runtime behind the scenes. For example, when this main runs to print Hello World
    // a thread object is created to handle that work.

    Console.WriteLine($"Main runs on thread #{Environment.CurrentManagedThreadId}");

    // We can create our own threads - using the Thread class. It's constructor takes one argument
    // It takes a delegate (we can define with a lambda OR pass it some prewritten method) to run
    // inside the thread
    var workerThread = new Thread(() =>
    {
        Console.WriteLine($"Hello from Thread #{Environment.CurrentManagedThreadId}");
    });

    // Once we have a thread setup - we have to manually start it
    Console.WriteLine($"Before Start() call, isAlive = {workerThread.IsAlive}");  // Unstarted

    workerThread.Start(); // Thread is now Running
    Console.WriteLine($"During thread delegate code, isAlive = {workerThread.IsAlive}");  
    workerThread.Join(); // Our thread was called from the Main function's thread
    // Calling .Join() blocks the outer/caller thread similar to an await

    Console.WriteLine($"After Join() call, isAlive = {workerThread.IsAlive}");  // Stopped


    // Parallelism vs Concurrency
    // Interleaving * Below even the runtime the actual OS scheduler (the thing the kernel uses to map
    // OS threads to CPU threads) interleaves the threads - switches them on and off CPU threads really fast
    // according to rules that we can't influence from our program - so our threads don't really complete
    // in same order 100% of the time. This can make our code non-deterministic - with os a problem

    // Concurrency - task in progress (interleaved, even one one CPU core)
    // Parallelism - tasks executing at the same time (multiple cpu cores)

    // Threads give us concurrency, true parallelism depends on hardware (and kernel)

    var threads = new List<Thread>(); // Empty list of threads

    // Lets just use a loop to create a few really fast
    for (int i = 0; i <= 5; i++)
    {
        int id = i;
         var th = new Thread(() =>
         {
             Thread.Sleep(Random.Shared.Next(5,40)); // Simulationg some work
             Console.WriteLine($"Worker {id} finished on thread #{Environment.CurrentManagedThreadId}");
         });

         threads.Add(th);
         th.Start();
    }

    foreach(Thread thread in threads) thread.Join(); // Join call  on each thread

    Console.WriteLine("\n============================ THREAD SAFE COLLECTION ============================\n");

    // Thread Safe collection

    // Ordinary collections are not optimized or built with multiple threads in mind - they would corrupt or
    // more likely throw exceptions if two thread delegates accessed them concurrently.
    // Thnakfully there are thread safe versions of comon collections and methods
    var counts = new ConcurrentDictionary<int, int>();

    var threadPool = new List<Thread>(); // list for our threads
    
    for (int i =1; i <= 8; i++)
    {
        
        int id = i;
        var th = new Thread(() =>
        {
            for (int k =0; k < 1000; k++)
                counts.AddOrUpdate(id, 1, (_, prev) => prev + 1);
                // In the line above, AddOrUpdate takes the key, the value and a third argument
                // a delegate to execute if the key already exists
                // _ = C# discard - indicates the key parameter is intentionally ignored because the 
                // delegate won't use it
                // prev - the existing integer value currently stored for that key
                // prev + 1 = increment that value giving us a new key to insert
        });

        threadPool.Add(th);
        th.Start();
    }
    

    foreach(var th  in threadPool) th.Join(); // Join to block main's thread
    Console.WriteLine($"Recorded {counts.Values.Sum()} increments across {counts.Count} threads");

    // When working woth threads, it's common to not manually create the threads ourselves
    // For short work items like what we did above, we can use the ThreadPoll
    // The ThreadPoll os just a runtime managed set of background threads that we don't have to
    // create or destroy - they're already there we can just borrow one.

    // Lets make a ConcurrentQueue for FIFO work, we'll just have it store ints
    var done = new ConcurrentQueue<int>();

    for (int i = 0; i < 5; i++)
    {
        int n = i;

        // Instead of creating a Thread manually and starting it I can just ask for a thread from
        // the background ThreadPool and pass it some delegate or method to execute
        ThreadPool.QueueUserWorkItem(_ => done.Enqueue(n*n));
    }

    // Because we don't actually have the Threads themselves at our disposal - we'll
    // do like a crude await
    while(done.Count < 5) Thread.Sleep(5); // await - but way dumber

    Console.WriteLine($"ThreadPool finished. {string.Join(", ", done.OrderBy(x => x))}");

    // Tasks. We've already seen Tasks. creating Threads, Starting and Joining them manually works.
    // But its very low level. You manage weach thread, you can't return a value in a straightforward way,
    // etc. Thankfully we have the Task Parallel library. Its like a modern layer ontop.
    ParallelSum();

    static void ParallelSum()
    {
        Console.WriteLine("\n============================ ParallelSum ============================\n");
        int numbers = 5;
        // Just a big int array
        int[] data = Enumerable.Range(1, numbers).ToArray();
        Console.WriteLine($"\nNumber of numbers to sum: {data.Length}");

        // First - lets do this totally sequentially - one thread without tasks
        var sw = Stopwatch.StartNew(); // using a Stopwatch object to track execution time
        long sequetial = SumRange(data, 0, data.Length);
        sw.Stop();
        Console.WriteLine($"Sequential sum = {sequetial}. {sw.ElapsedTicks} ticks, 1 thread");


        sw.Restart();

        // Before we parallelise this, lets play with Tasks
        // Manually splitting the summing into two Tasks, each gets half the total numbers
        Task<long> half1 = Task.Run(() => SumRange(data, 0, data.Length/2));
        Task<long> half2 = Task.Run(() => SumRange(data, data.Length/2, data.Length));

        long total = half1.Result + half2.Result; // Asking for the result of a Task is blocking
        sw.Stop();
        Console.WriteLine($"Two Task sum = {total}. {sw.ElapsedTicks} ticks");


        // Lets parallelise this with Tasks and the TPL library
        long parallelTotal = 0;

        sw.Restart(); // restarting my stopwatch back to 0 ticks - then begin counting
        Parallel.For(0, data.Length, 
            // After we give it start and end values for the llop - this is a For loop
            // We give it an accumulator
            () => 0L,
            // body for each iteration on a given thread do something
            // i is the loop index, _ discards the ParallelLoopState, local is the current
            (i, _, local) => local + data[i],
            // localFinally: AFTER the Thread's local Sum (the thing that starts with a value of 0L (long))
            // to the glopal parallelTotal
            local => Interlocked.Add(ref parallelTotal, local) // combine per Thread sums to the outer variable
        );
        sw.Stop();
        Console.WriteLine($"Parallel sum = {parallelTotal}. {sw.ElapsedTicks} ticks, multi-thread");

    }


    static long SumRange(int[] a, int start, int end)
    {
        long sum = 0;
        for (int i = start; i < end; i++) sum += a[i];

        return sum;
    }
    Console.WriteLine("\n============================ Bank Simulation ============================\n");
    RaceDemo(); // Creates a race condition
    SafeDemo();

    static void RaceDemo()
    {
        var bank = new Bank();
        Parallel.For(0, 100000, _ => bank.DepositUnsafe(1)); // 100k threads worth of + 1
        Console.WriteLine($"Unsafe balance = {bank.Balance} (expected 100000)");
        // Our balance is wrong every time - and it's a different wrong answer every time
        // This is the worst kind of bug. Because its not deterministic
    }

    static void SafeDemo()
    {
        var bank = new Bank();
        Parallel.For(0, 100000, _ => bank.DepositSafe(1));
        Console.WriteLine($"Safe balance = {bank.Balance} (expected 100000)");
    }

    // Interlocked - lock free atomic operations against one variable

    InterlockedDemo();

    static void InterlockedDemo()
    {
        long counter = 0;
        // Interlocked - faster than a lock when doing single atomic operations
        // if all you need is that - use an interlock over a lock
        Parallel.For(0, 100000, _ => Interlocked.Increment(ref counter));
        Console.WriteLine($"Interlocked = {counter} (expected 100000)");
    }

    // Deadlock and Starvation

    // Deadlock - If two tasks create locks on resources the other ends up needing
    // they can deadlock. In this case, they never resolve - our console app
    // would be wating forever

    // Starvation - A thread gets blocked by another threads work - and stays alive
    // but cannot progress. Different from a deathlock - because the other thread is able to resolve
    // This starved threads persists - potentially starving the ThreadPool

    // Cancellation Tokens

    CancellationTokens();

    // Rather than abruptally killing a thread or having it die via some exception
    // potentially leading to data loss - we can use a cancellation token to ASK a thread to be ended
    // and it will do so once it has the chance to exit gracefully

    static void CancellationTokens()
    {
        // Calling for a CancellationToken, having it auto-cancel after 100ms
        // Side not using: Once we exit the scope where the variable created with using
        // lives in - dispose of it
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        CancellationToken token = cts.Token;

        var work = Task.Run(() =>
        {
            for(long i = 0; ; i++)
            {
                token.ThrowIfCancellationRequested();
                if(i % 5000000 == 0) { /* Some simulated work */}
            }
        }, token);

        try
        {
            work.Wait(); // The task is going - we want to have our code wait for it here
        } // Exeption filtering - for when exceptions are thrown by other exceptions
        catch (AggregateException ex) when (ex.InnerException is OperationCanceledException)
        {
            Console.WriteLine("Work was cancelled cooperatively");
        }   
        // When doing Task Parallel Library stuff, we need to jump unwrap the AggregateExceptions
        // To allow for specific catch. Same logic as multiple catch blocks
        // Just more convoluted because AggregateExceptions are like an exception list
        catch (AggregateException ex) when (ex.InnerException is InvalidOperationException)
        {
            Console.WriteLine("How'd you get here>?");
        }
    }


    ExceptionDemo();

    static void ExceptionDemo()
    {
        var t = Task.Run(() => throw new InvalidOperationException("oops - but in a task"));

        // Counter-intuitively, an exception inside a task DOESN'T crash on the spot
        // We'd imagin that line 279 is where the exception is thrown. Its actually
        // thrown dirong the t.Wait() below

        try
        {
            t.Wait();
        }
        catch ( AggregateException ex)
        {   // Aggregate exceptions themselves are kind of weird
            // One task can have several faults - so they get thrown inside an AggregateException
            Console.WriteLine($"Caught: {ex.InnerException!.Message}");
        }
    }

    // Async / await - related to but not same as a thread
    await AsyncDemo();

    static async Task AsyncDemo()
    {
        Console.WriteLine($"Before await on thread #{Environment.CurrentManagedThreadId}");
        await Task.Delay(50); // non blocking wait - thread is free
        Console.WriteLine($"After await on thread #{Environment.CurrentManagedThreadId}");
    }
    
}