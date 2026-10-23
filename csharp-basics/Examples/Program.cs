using System;

namespace CSharpBasics.Examples
{
    // Start with F5 or double-click Examples.exe. Type a topic number and press Enter. Q quits.
    // From a terminal you can also skip the menu:
    //   dotnet run --project Examples -- 3       -> run topic 3
    //   dotnet run --project Examples -- all     -> run everything
    public static class Program
    {
        private static readonly (string Name, Action Run)[] Topics =
        {
            ("Types: value vs reference, struct vs class", TypesAndValues.Run),
            ("Properties and methods", PropertiesAndMethods.Run),
            ("Interfaces, inheritance vs composition", InterfacesAndComposition.Run),
            ("Generics", Generics.Run),
            ("Collections: array, List, Dictionary", Collections.Run),
            ("Delegates, events, Action/Func, lambdas, closures", DelegatesAndEvents.Run),
            ("Nullable value types and null handling", Nullables.Run),
            ("Exceptions", Exceptions.Run),
            ("Garbage collector: what is an allocation?", GarbageCollection.Run),
            ("LINQ (and why not in hot paths)", Linq.Run),
            ("async/await (and how it maps to Unity)", AsyncAwait.Run),
            ("Syntactic sugar: ?: ?. ?? event?.Invoke and friends", SyntacticSugar.Run),
        };

        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                RunMenu();
                return 0;
            }

            if (args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < Topics.Length; i++) RunTopic(i);
                return 0;
            }

            // TryParse, not Parse + catch: exceptions are not control flow (lecture 3.2).
            if (TryGetTopic(args[0], out var index))
            {
                RunTopic(index);
                return 0;
            }

            Console.Error.WriteLine($"Unknown topic '{args[0]}'. Use a number from 1 to {Topics.Length}, or all.");
            return 1;
        }

        private static void RunMenu()
        {
            PrintMenu();
            while (true)
            {
                Console.WriteLine();
                Console.Write($"Topic (1-{Topics.Length}, L = list, A = all, Q = quit): ");
                var input = Console.ReadLine();

                // ReadLine returns null when the window is closed or input ends.
                if (input == null) return;
                input = input.Trim();

                if (input.Equals("q", StringComparison.OrdinalIgnoreCase)) return;

                if (input.Equals("l", StringComparison.OrdinalIgnoreCase))
                    PrintMenu();
                else if (input.Equals("a", StringComparison.OrdinalIgnoreCase))
                    for (var i = 0; i < Topics.Length; i++) RunTopic(i);
                else if (TryGetTopic(input, out var index))
                    RunTopic(index);
                else
                    Console.WriteLine($"'{input}' isn't a topic. Type a number from 1 to {Topics.Length}.");
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("C# basics for Unity developers. Pick a topic:");
            Console.WriteLine();
            for (var i = 0; i < Topics.Length; i++)
                Console.WriteLine($"  {i + 1,2}. {Topics[i].Name}");
            Console.WriteLine();
        }

        private static bool TryGetTopic(string text, out int index)
        {
            index = -1;
            if (!int.TryParse(text, out var number) || number < 1 || number > Topics.Length) return false;
            index = number - 1;
            return true;
        }

        private static void RunTopic(int index)
        {
            Console.WriteLine();
            Console.WriteLine($"=== {index + 1}. {Topics[index].Name} ===");
            Topics[index].Run();
        }
    }
}