using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreadingQuete1
{
    class Program
    {
        private Int32 SharedInteger { get; set; }
        private Mutex mutex = new Mutex();

        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            var threadStartDelegate = new ThreadStart(OnThreadStart);
            var threads = new List<Thread>();

            Thread thread1 = new Thread(threadStartDelegate)
            {
                Name = "Thread1",
            };

            Thread thread2 = new Thread(threadStartDelegate)
            {
                Name = "Thread2"
            };

            threads.Add(thread1);
            threads.Add(thread2);

            foreach (var t in threads)
            {
                t.Start(); // We launch the thread
            }

            foreach (var t in threads)
            {
                t.Join(); // We wait until all threads are finished
            }
        }

        private void OnThreadStart()
        {
            var random = new System.Random();
            var executionTime = random.Next(2, 7);
            var timeSpan = TimeSpan.FromSeconds(executionTime);
            Console.WriteLine("\nCurrent thread name: {0} , and current thread id : {1} started", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);

            mutex.WaitOne(); // Wait for the mutex to be released
            SharedInteger += timeSpan.Milliseconds;
            Thread.Sleep(timeSpan); // Simulate computing by waiting a random period of time
            mutex.ReleaseMutex(); // Release when computing is done
            Console.WriteLine("\nCurrent thread name: {0} , and current thread id : {1} ", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
