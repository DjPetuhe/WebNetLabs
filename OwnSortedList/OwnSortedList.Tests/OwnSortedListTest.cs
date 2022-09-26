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
        public void AddTest()
        {
            OwnSortedList<int, string> test = new();
            test.Add(5, "a");
            test.Add(-1, "b");
            test.Add(make(2, "c")); ;
            Assert.Equal(new[] {make(-1, "b"), make(2, "c"), make(5, "a")}, test);
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
        public void ContainsTest()
        {
            OwnSortedList<string, string> test = new()
            {
                { "key1", "a" },
                { "key2", "b" }
            };
            Assert.True(test.ContainsKey("key1"));
            Assert.True(test.Contains(make("key2", "b")));
            Assert.False(test.ContainsKey("key3"));
            Assert.False(test.Contains(make("key1", "b")));
        }

        [Fact]
        public void RemoveTest()
        {
            OwnSortedList<char, bool> test = new()
            {
                { 'a', true },
                { 'b', false },
                { 'c', true},
                { 'd', false },
            };
            test.Remove('c');
            test.Remove(make('b', false));
            Assert.False(test.Remove('e'));
            Assert.Equal(new[] { make('a', true), make('d', false)}, test);
        }

        [Fact]
        public void TryGetTest()
        {
            OwnSortedList<int, int> test = new(Comparer<int>.Default)
            {
                { 10, 0 },
                { 4, 22 },
                { -2, 3 },
            };
            int value;
            test.TryGetValue(-2, out value);
            Assert.Equal(3, value);
            test.TryGetValue(2, out value);
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
    }
}