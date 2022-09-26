using OwnSortedList;

namespace MyStructureShow
{
    class Program
    {
        static void Main()
        {
            {
                OwnSortedList<int, string> test = new()
                {
                    { 5, "a" },
                    { 2, "b" },
                    { 3, "c" },
                    { 1, "d" },
                    { 4, "e" }
                };
                test.ElementAdded += InformAboutAdding;
                test.ValueChanged += InformAboutChanging;
                test.ElementDeleted += InformAboutDeleting;
                test.ListCleared += InformAboutClear;
                Console.WriteLine(new String('-', 20));
                test.Add(-1, "f");
                test[3] = "-c";
                test.Remove(1);
                Console.WriteLine(new String('-', 20));
                foreach (var el in test)
                {
                    Console.WriteLine($"Key:{el.Key} Value: {el.Value}");
                }
                Console.WriteLine(new String('-', 20));
                test.Clear();
                Console.WriteLine(new String('-', 20));
            }
        }
        static void InformAboutAdding<TKey, TValue>(TKey k, TValue v) => Console.WriteLine($"Key: {k}, Value: {v} has been added");
        static void InformAboutChanging<TKey, TValue>(TKey k, TValue v) => Console.WriteLine($"Value of element with Key: {k} has been changed to {v}");
        static void InformAboutDeleting<TKey, TValue>(TKey k, TValue v) => Console.WriteLine($"Key: {k}, Value: {v} has been deleted");
        static void InformAboutClear(int size) => Console.WriteLine($"Sorted list with {size} elements has been cleared");
    }
}