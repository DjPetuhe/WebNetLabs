using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace OwnSortedList
{
    public class OwnSortedList<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        private TKey[] keys;
        private TValue[] values;
        private IComparer<TKey> comp;
        private int size;
        private int realSize;
        private const int startSize = 4;
        public event Action<TKey, TValue>? ElementAdded;
        public event Action<TKey, TValue>? ValueChanged;
        public event Action<TKey, TValue>? ElementDeleted;
        public event Action<int>? ListCleared;
        public bool IsReadOnly { get; } = false;
        public int Count
        {
            get { return size; }
            private set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();
                size = value;
                if (size >= realSize)
                {
                    realSize *= 2;
                    Array.Resize<TKey>(ref keys, realSize);
                    Array.Resize<TValue>(ref values, realSize);
                }
            }
        }
        public OwnSortedList() : this(Comparer<TKey>.Default, startSize) { }
        public OwnSortedList(int realSize) : this(Comparer<TKey>.Default, realSize) { }
        public OwnSortedList(IComparer<TKey> comparer) : this(comparer, startSize) { }
        public OwnSortedList(IComparer<TKey> comparer, int realSize)
        {
            if (comparer is null) throw new ArgumentNullException();
            if (realSize < 0) throw new ArgumentOutOfRangeException();
            this.realSize = realSize;
            this.Count = 0;
            this.keys = new TKey[realSize];
            this.values = new TValue[realSize];
            this.comp = comparer;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (ContainsKey(key)) return values[KeyIndex(key)];
                throw new KeyNotFoundException();
            }
            set
            {
                int index = Array.BinarySearch<TKey>(keys, 0, Count, key, comp);
                if (index >= 0)
                {
                    values[index] = value;
                    ValueChanged?.Invoke(key, value);
                }
                else Add(~index, key, value);
            }
        }

        private void Add(int index, TKey key, TValue value)
        {
            if (index < Count)
            {
                Array.Copy(keys, index, keys, index + 1, Count - index);
                Array.Copy(values, index, values, index + 1, Count - index);
            }
            keys[index] = key;
            values[index] = value;
            Count++;
            ElementAdded?.Invoke(key, value);
        }

        public void Add(TKey key, TValue value)
        {
            if (key is null) throw new ArgumentNullException();
            if (ContainsKey(key)) throw new ArgumentException();
            int index = Array.BinarySearch<TKey>(keys, 0, Count, key, comp);
            Add(~index, key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        private int KeyIndex(TKey key)
        {
            if (key is null) throw new ArgumentNullException("Key element is null");
            int index = Array.BinarySearch<TKey>(keys, 0, Count, key, comp);
            return index >= 0 ? index : -1;
        }

        public void Clear()
        {
            Array.Clear(keys, 0, realSize);
            Array.Clear(values, 0, realSize);
            int save = Count;
            realSize = 0;
            Count = 0;
            ListCleared?.Invoke(save);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (ContainsKey(item.Key))
            {
                int index = Array.BinarySearch<TKey>(keys, 0, Count, item.Key, comp);
                if (EqualityComparer<TValue>.Default.Equals(values[index], item.Value)) return true;
            }
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            return KeyIndex(key) >= 0;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array is null) throw new ArgumentNullException();
            if (array.Length - arrayIndex < Count) throw new ArgumentException();
            if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException();
            for (int i = 0; i < Count; i++) array[arrayIndex + i] = new KeyValuePair<TKey, TValue>(keys[i], values[i]);
        }

        public bool Remove(TKey key)
        {
            if (!ContainsKey(key)) return false;
            int index = KeyIndex(key);
            TValue save = values[index];
            Count--;
            if (Count > index)
            {
                Array.Copy(keys, index + 1, keys, index, size - index);
                Array.Copy(values, index + 1, values, index, size - index);
            }
            keys[Count] = default;
            values[Count] = default;
            ElementDeleted?.Invoke(key, save);
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (ContainsKey(key))
            {
                value = values[KeyIndex(key)];
                return true;
            }
            value = default;
            return false;
        }

        public ICollection<TKey> Keys => new ArraySegment<TKey>(this.keys, 0, size);

        public ICollection<TValue> Values => new ArraySegment<TValue>(this.values, 0, size);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < size; i++)
            {
                yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
