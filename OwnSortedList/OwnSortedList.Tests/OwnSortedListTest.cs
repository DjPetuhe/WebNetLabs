using Newtonsoft.Json.Linq;
using OwnSortedList;
using System.Reflection.Metadata.Ecma335;
using Xunit;
using Xunit.Sdk;

namespace OwnSortedList.Tests
{
    public class OwnSortedListTest
    {
        private KeyValuePair<KeyT, ValueT> make<KeyT, ValueT>(KeyT a, ValueT b) => new(a, b);

        [Fact]
        public void NegativeCapacityExceptionTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OwnSortedList<int,int>(-1));
        }

        [Fact]
        public void NullComparerExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() => new OwnSortedList<int, int>(null, 10));
        }

        [Fact]
        public void AddTest()
        {
            OwnSortedList<int, string> test = new();
            test.Add(5, "a");
            test.Add(-1, "b");
            test.Add(2, "c");
            Assert.Equal(new[] {make(-1, "b"), make(2, "c"), make(5, "a")}, test);
        }

        [Fact]
        public void AddPairTest()
        {
            OwnSortedList<int, string> test = new();
            test.Add(make(5, "a"));
            test.Add(make(-1, "b"));
            test.Add(make(2, "c"));
            Assert.Equal(new[] { make(-1, "b"), make(2, "c"), make(5, "a") }, test);
        }

        [Fact]
        public void AddNullException()
        {
            OwnSortedList<string, string> test = new();
            Assert.Throws<ArgumentNullException>(() => test.Add(null, "5"));
        }

        [Fact]
        public void AddExistingException()
        {
            OwnSortedList<int, int> test = new()
            {
                {10, 30 }
            };
            Assert.Throws<ArgumentException>(() => test.Add(10, 10));
        }

        [Fact]
        public void ClearTest()
        {
            OwnSortedList<int, bool> test = new(10)
            {
                { 1, true },
                { 2, false }
            };
            test.Clear();
            Assert.Empty(test);
        }

        [Fact]
        public void ContainsKeyTest()
        {
            OwnSortedList<string, string> test = new()
            {
                { "key1", "a" },
                { "key2", "b" }
            };
            Assert.True(test.ContainsKey("key1"));
            Assert.False(test.ContainsKey("key3"));
        }

        [Fact]
        public void ContainsTest()
        {
            OwnSortedList<string, string> test = new()
            {
                { "key1", "a" },
                { "key2", "b" }
            };
            Assert.True(test.Contains(make("key2", "b")));
            Assert.False(test.Contains(make("key1", "b")));
        }

        [Fact]
        public void RemoveExistingTest()
        {
            OwnSortedList<char, bool> test = new()
            {
                { 'a', true },
                { 'b', false },
                { 'c', true },
                { 'd', false },
            };
            test.Remove('c');
            test.Remove('b');
            Assert.Equal(new[] { make('a', true), make('d', false)}, test);
        }

        [Fact]
        public void RemoveNotExistingTest()
        {
            OwnSortedList<char, bool> test = new()
            {
                { 'a', true },
                { 'b', false },
            };
            Assert.False(test.Remove('c'));
        }

        [Fact]
        public void RemoveExistingPairTest()
        {
            OwnSortedList<char, bool> test = new()
            {
                { 'a', true },
                { 'b', false },
                { 'c', true },
                { 'd', false },
            };
            test.Remove(make('c', true));
            test.Remove(make('b', false));
            Assert.False(test.Remove(make('e', true)));
            Assert.Equal(new[] { make('a', true), make('d', false) }, test);
        }

        [Fact]
        public void RemoveNotExistingPairTest()
        {
            OwnSortedList<char, bool> test = new()
            {
                { 'a', true },
                { 'b', false },
            };
            Assert.False(test.Remove(make('e', true)));
        }

        [Fact]
        public void TryGetExistingTest()
        {
            OwnSortedList<int, int> test = new(Comparer<int>.Default)
            {
                { 10, 0 },
                { 4, 22 },
                { -2, 3 },
            };
            test.TryGetValue(-2, out int value);
            Assert.Equal(3, value);
        }

        [Fact]
        public void TryGetNotExistingTest()
        {
            OwnSortedList<int, int> test = new(Comparer<int>.Default)
            {
                { 10, 0 },
                { 4, 22 },
                { -2, 3 },
            };
            test.TryGetValue(2, out int value);
            Assert.Equal(default, value);
        }

        [Fact]
        public void IndexationTest()
        {
            OwnSortedList<int, double> test = new()
            {
                {1, 2.5 },
                {10, 25.5 },
            };
            List<int> keys = test.Keys.ToList();
            foreach (int key in keys)
            {
                test[key] = test[key] * 10;
            }
            test[100] = 2555;
            Assert.Equal(new[] { make(1, (double)25), make(10, (double)255), make(100, (double)2555) }, test);
        }

        [Fact]
        public void IndexationGetException()
        {
            OwnSortedList<int, int> test = new() { {1, 2 } };
            int temp;
            Assert.Throws<KeyNotFoundException>(() => temp = test[2]);
        }

        [Fact]
        public void IndexationNullException()
        {
            OwnSortedList<string, int> test = new() { { "1", 2 } };
            int temp;
            Assert.Throws<ArgumentNullException>(() => temp = test[null]);
        }

        [Fact]
        public void CopyToTest()
        {
            OwnSortedList<int, string> test = new()
            {
                { 1, "first" },
                { 2, "second" },
                { 3, "third" },
            };
            List<string> values = test.Values.ToList();
            KeyValuePair<int, string>[] temp = new KeyValuePair<int, string>[values.Count];
            test.CopyTo(temp, 0);
            for (int i = 0; i < temp.Length; i++)
            {
                Assert.Equal(temp[i].Value, values[i]);
            }
        }

        [Fact]
        public void CopyToNullExceptionTest()
        {
            OwnSortedList<int, int> test = new() { { 1, 2 } };
            Assert.Throws<ArgumentNullException>(() => test.CopyTo(null, 0));
        }

        [Fact]
        public void CopyToIndexExceptionTest()
        {
            OwnSortedList<int, string> test = new()
            {
                { 1, "first" },
                { 2, "second" },
                { 3, "third" },
            };
            KeyValuePair<int, string>[] temp = new KeyValuePair<int, string>[3];
            Assert.Throws<ArgumentException>(() => test.CopyTo(temp, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => test.CopyTo(temp, -1));
        }
    }
}