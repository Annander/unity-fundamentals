using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpBasics.Examples
{
    public static class AsyncAwait
    {
        public static void Run()
        {
            // A console app has no Unity main loop; block here just for the demo.
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            // 'await' pauses THIS method without blocking the thread; the rest runs when the task completes.
            Console.WriteLine("Loading...");
            int size = await LoadLevelAsync("forest", CancellationToken.None);
            Console.WriteLine($"Loaded level, size={size}");

            // Cancellation is cooperative: pass a token, check it, let OperationCanceledException propagate.
            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(TimeSpan.FromMilliseconds(20));
                try
                {
                    await LoadLevelAsync("huge-level", cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Load cancelled (as intended)");
                }
            }

            // Run several in parallel and wait for all.
            int[] sizes = await Task.WhenAll(LoadLevelAsync("a", default), LoadLevelAsync("b", default));
            Console.WriteLine($"Loaded {sizes.Length} levels concurrently");
        }

        private static async Task<int> LoadLevelAsync(string name, CancellationToken token)
        {
            for (int i = 0; i < 5; i++)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(10, token);
            }
            return name.Length * 100;
        }
    }
}