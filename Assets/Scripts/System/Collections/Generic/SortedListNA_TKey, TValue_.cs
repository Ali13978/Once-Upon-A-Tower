using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

namespace System.Collections.Generic
{
	[Serializable]
	[ComVisible(false)]
	[DebuggerDisplay("Count={Count}")]
	public class SortedListNA<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, ICollection, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		private enum EnumeratorMode
		{
			KEY_MODE,
			VALUE_MODE,
			ENTRY_MODE
		}

		private sealed class Enumerator : ICloneable, IDictionaryEnumerator, IEnumerator
		{
			private SortedListNA<TKey, TValue> host;

			private int stamp;

			private int pos;

			private int size;

			private EnumeratorMode mode;

			private object currentKey;

			private object currentValue;

			private bool invalid;

			private static readonly string xstr = "SortedListNA.Enumerator: snapshot out of sync.";

			public DictionaryEntry Entry
			{
				get
				{
					if (invalid || pos >= size || pos == -1)
					{
						throw new InvalidOperationException(xstr);
					}
					return new DictionaryEntry(currentKey, currentValue);
				}
			}

			public object Key
			{
				get
				{
					if (invalid || pos >= size || pos == -1)
					{
						throw new InvalidOperationException(xstr);
					}
					return currentKey;
				}
			}

			public object Value
			{
				get
				{
					if (invalid || pos >= size || pos == -1)
					{
						throw new InvalidOperationException(xstr);
					}
					return currentValue;
				}
			}

			public object Current
			{
				get
				{
					if (invalid || pos >= size || pos == -1)
					{
						throw new InvalidOperationException(xstr);
					}
					switch (mode)
					{
					case EnumeratorMode.KEY_MODE:
						return currentKey;
					case EnumeratorMode.VALUE_MODE:
						return currentValue;
					case EnumeratorMode.ENTRY_MODE:
						return Entry;
					default:
						throw new NotSupportedException(mode + " is not a supported mode.");
					}
				}
			}

			public Enumerator(SortedListNA<TKey, TValue> host, EnumeratorMode mode)
			{
				this.host = host;
				stamp = host.modificationCount;
				size = host.Count;
				this.mode = mode;
				Reset();
			}

			public Enumerator(SortedListNA<TKey, TValue> host)
				: this(host, EnumeratorMode.ENTRY_MODE)
			{
			}

			public void Reset()
			{
				if (host.modificationCount != stamp || invalid)
				{
					throw new InvalidOperationException(xstr);
				}
				pos = -1;
				currentKey = null;
				currentValue = null;
			}

			public bool MoveNext()
			{
				if (host.modificationCount != stamp || invalid)
				{
					throw new InvalidOperationException(xstr);
				}
				KeyValuePair<TKey, TValue>[] table = host.table;
				if (++pos < size)
				{
					KeyValuePair<TKey, TValue> keyValuePair = table[pos];
					currentKey = keyValuePair.Key;
					currentValue = keyValuePair.Value;
					return true;
				}
				currentKey = null;
				currentValue = null;
				return false;
			}

			public object Clone()
			{
				Enumerator enumerator = new Enumerator(host, mode);
				enumerator.stamp = stamp;
				enumerator.pos = pos;
				enumerator.size = size;
				enumerator.currentKey = currentKey;
				enumerator.currentValue = currentValue;
				enumerator.invalid = invalid;
				return enumerator;
			}
		}

		[Serializable]
		private struct KeyEnumerator : IEnumerator<TKey>, IDisposable, IEnumerator
		{
			private const int NOT_STARTED = -2;

			private const int FINISHED = -1;

			private SortedListNA<TKey, TValue> l;

			private int idx;

			private int ver;

			object IEnumerator.Current => Current;

			public TKey Current
			{
				get
				{
					if (idx < 0)
					{
						throw new InvalidOperationException();
					}
					return l.KeyAt(l.Count - 1 - idx);
				}
			}

			internal KeyEnumerator(SortedListNA<TKey, TValue> l)
			{
				this.l = l;
				idx = -2;
				ver = l.modificationCount;
			}

			public void Dispose()
			{
				idx = -2;
			}

			public bool MoveNext()
			{
				if (ver != l.modificationCount)
				{
					throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
				}
				if (idx == -2)
				{
					idx = l.Count;
				}
				return idx != -1 && --idx != -1;
			}

			void IEnumerator.Reset()
			{
				if (ver != l.modificationCount)
				{
					throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
				}
				idx = -2;
			}
		}

		[Serializable]
		private struct ValueEnumerator : IEnumerator<TValue>, IDisposable, IEnumerator
		{
			private const int NOT_STARTED = -2;

			private const int FINISHED = -1;

			private SortedListNA<TKey, TValue> l;

			private int idx;

			private int ver;

			object IEnumerator.Current => Current;

			public TValue Current
			{
				get
				{
					if (idx < 0)
					{
						throw new InvalidOperationException();
					}
					return l.ValueAt(l.Count - 1 - idx);
				}
			}

			internal ValueEnumerator(SortedListNA<TKey, TValue> l)
			{
				this.l = l;
				idx = -2;
				ver = l.modificationCount;
			}

			public void Dispose()
			{
				idx = -2;
			}

			public bool MoveNext()
			{
				if (ver != l.modificationCount)
				{
					throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
				}
				if (idx == -2)
				{
					idx = l.Count;
				}
				return idx != -1 && --idx != -1;
			}

			void IEnumerator.Reset()
			{
				if (ver != l.modificationCount)
				{
					throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
				}
				idx = -2;
			}
		}

		private class ListKeys : IList<TKey>, ICollection, IEnumerable, ICollection<TKey>, IEnumerable<TKey>
		{
			private SortedListNA<TKey, TValue> host;

			public virtual TKey this[int index]
			{
				get
				{
					return host.KeyAt(index);
				}
				set
				{
					throw new NotSupportedException("attempt to modify a key");
				}
			}

			public virtual int Count => host.Count;

			public virtual bool IsSynchronized => ((ICollection)host).IsSynchronized;

			public virtual bool IsReadOnly => true;

			public virtual object SyncRoot => ((ICollection)host).SyncRoot;

			public ListKeys(SortedListNA<TKey, TValue> host)
			{
				if (host == null)
				{
					throw new ArgumentNullException();
				}
				this.host = host;
			}

			public virtual void Add(TKey item)
			{
				throw new NotSupportedException();
			}

			public virtual bool Remove(TKey key)
			{
				throw new NotSupportedException();
			}

			public virtual void Clear()
			{
				throw new NotSupportedException();
			}

			public virtual void CopyTo(TKey[] array, int arrayIndex)
			{
				if (host.Count != 0)
				{
					if (array == null)
					{
						throw new ArgumentNullException("array");
					}
					if (arrayIndex < 0)
					{
						throw new ArgumentOutOfRangeException();
					}
					if (arrayIndex >= array.Length)
					{
						throw new ArgumentOutOfRangeException("arrayIndex is greater than or equal to array.Length");
					}
					if (Count > array.Length - arrayIndex)
					{
						throw new ArgumentOutOfRangeException("Not enough space in array from arrayIndex to end of array");
					}
					int num = arrayIndex;
					for (int i = 0; i < Count; i++)
					{
						array[num++] = host.KeyAt(i);
					}
				}
			}

			public virtual bool Contains(TKey item)
			{
				return host.IndexOfKey(item) > -1;
			}

			public virtual int IndexOf(TKey item)
			{
				return host.IndexOfKey(item);
			}

			public virtual void Insert(int index, TKey item)
			{
				throw new NotSupportedException();
			}

			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			public virtual IEnumerator<TKey> GetEnumerator()
			{
				return new KeyEnumerator(host);
			}

			public virtual void CopyTo(Array array, int arrayIndex)
			{
				host.CopyToArray(array, arrayIndex, EnumeratorMode.KEY_MODE);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				for (int i = 0; i < host.Count; i++)
				{
					yield return host.KeyAt(i);
				}
			}
		}

		private class ListValues : IList<TValue>, ICollection, IEnumerable, ICollection<TValue>, IEnumerable<TValue>
		{
			private SortedListNA<TKey, TValue> host;

			public virtual TValue this[int index]
			{
				get
				{
					return host.ValueAt(index);
				}
				set
				{
					throw new NotSupportedException("attempt to modify a key");
				}
			}

			public virtual int Count => host.Count;

			public virtual bool IsSynchronized => ((ICollection)host).IsSynchronized;

			public virtual bool IsReadOnly => true;

			public virtual object SyncRoot => ((ICollection)host).SyncRoot;

			public ListValues(SortedListNA<TKey, TValue> host)
			{
				if (host == null)
				{
					throw new ArgumentNullException();
				}
				this.host = host;
			}

			public virtual void Add(TValue item)
			{
				throw new NotSupportedException();
			}

			public virtual bool Remove(TValue value)
			{
				throw new NotSupportedException();
			}

			public virtual void Clear()
			{
				throw new NotSupportedException();
			}

			public virtual void CopyTo(TValue[] array, int arrayIndex)
			{
				if (host.Count != 0)
				{
					if (array == null)
					{
						throw new ArgumentNullException("array");
					}
					if (arrayIndex < 0)
					{
						throw new ArgumentOutOfRangeException();
					}
					if (arrayIndex >= array.Length)
					{
						throw new ArgumentOutOfRangeException("arrayIndex is greater than or equal to array.Length");
					}
					if (Count > array.Length - arrayIndex)
					{
						throw new ArgumentOutOfRangeException("Not enough space in array from arrayIndex to end of array");
					}
					int num = arrayIndex;
					for (int i = 0; i < Count; i++)
					{
						array[num++] = host.ValueAt(i);
					}
				}
			}

			public virtual bool Contains(TValue item)
			{
				return host.IndexOfValue(item) > -1;
			}

			public virtual int IndexOf(TValue item)
			{
				return host.IndexOfValue(item);
			}

			public virtual void Insert(int index, TValue item)
			{
				throw new NotSupportedException();
			}

			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			public virtual IEnumerator<TValue> GetEnumerator()
			{
				return new ValueEnumerator(host);
			}

			public virtual void CopyTo(Array array, int arrayIndex)
			{
				host.CopyToArray(array, arrayIndex, EnumeratorMode.VALUE_MODE);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				for (int i = 0; i < host.Count; i++)
				{
					yield return host.ValueAt(i);
				}
			}
		}

		private static readonly int INITIAL_SIZE = 16;

		private int inUse;

		private int modificationCount;

		private KeyValuePair<TKey, TValue>[] table;

		private IComparer<TKey> comparer;

		private int defaultCapacity;

		bool ICollection.IsSynchronized => false;

		object ICollection.SyncRoot => this;

		bool IDictionary.IsFixedSize => false;

		bool IDictionary.IsReadOnly => false;

		object IDictionary.this[object key]
		{
			get
			{
				if (!(key is TKey))
				{
					return null;
				}
				return this[(TKey)key];
			}
			set
			{
				this[ToKey(key)] = ToValue(value);
			}
		}

		ICollection IDictionary.Keys => new ListKeys(this);

		ICollection IDictionary.Values => new ListValues(this);

		ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

		ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

		public int Count => inUse;

		public TValue this[TKey key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				int num = Find(key);
				if (num >= 0)
				{
					return table[num].Value;
				}
				throw new KeyNotFoundException();
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				PutImpl(key, value, overwrite: true);
			}
		}

		public int Capacity
		{
			get
			{
				return table.Length;
			}
			set
			{
				int num = table.Length;
				if (inUse > value)
				{
					throw new ArgumentOutOfRangeException("capacity too small");
				}
				if (value == 0)
				{
					KeyValuePair<TKey, TValue>[] destinationArray = new KeyValuePair<TKey, TValue>[defaultCapacity];
					Array.Copy(table, destinationArray, inUse);
					table = destinationArray;
				}
				else if (value > inUse)
				{
					KeyValuePair<TKey, TValue>[] destinationArray2 = new KeyValuePair<TKey, TValue>[value];
					Array.Copy(table, destinationArray2, inUse);
					table = destinationArray2;
				}
				else if (value > num)
				{
					KeyValuePair<TKey, TValue>[] destinationArray3 = new KeyValuePair<TKey, TValue>[value];
					Array.Copy(table, destinationArray3, num);
					table = destinationArray3;
				}
			}
		}

		public IList<TKey> Keys => new ListKeys(this);

		public IList<TValue> Values => new ListValues(this);

		public IComparer<TKey> Comparer => comparer;

		public SortedListNA()
			: this(INITIAL_SIZE, (IComparer<TKey>)null)
		{
		}

		public SortedListNA(int capacity)
			: this(capacity, (IComparer<TKey>)null)
		{
		}

		public SortedListNA(int capacity, IComparer<TKey> comparer)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity");
			}
			if (capacity == 0)
			{
				defaultCapacity = 0;
			}
			else
			{
				defaultCapacity = INITIAL_SIZE;
			}
			Init(comparer, capacity, forceSize: true);
		}

		public SortedListNA(IComparer<TKey> comparer)
			: this(INITIAL_SIZE, comparer)
		{
		}

		public SortedListNA(IDictionary<TKey, TValue> dictionary)
			: this(dictionary, (IComparer<TKey>)null)
		{
		}

		public SortedListNA(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			Init(comparer, dictionary.Count, forceSize: true);
			foreach (KeyValuePair<TKey, TValue> item in dictionary)
			{
				Add(item.Key, item.Value);
			}
		}

		public void Add(TKey key, TValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			PutImpl(key, value, overwrite: false);
		}

		public bool ContainsKey(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return Find(key) >= 0;
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			for (int i = 0; i < inUse; i++)
			{
				KeyValuePair<TKey, TValue> current = table[i];
				yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
			}
		}

		public bool Remove(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int num = IndexOfKey(key);
			if (num >= 0)
			{
				RemoveAt(num);
				return true;
			}
			return false;
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			inUse = 0;
			modificationCount++;
		}

		public void Clear()
		{
			inUse = 0;
			modificationCount++;
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (Count != 0)
			{
				if (array == null)
				{
					throw new ArgumentNullException();
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (arrayIndex >= array.Length)
				{
					throw new ArgumentNullException("arrayIndex is greater than or equal to array.Length");
				}
				if (Count > array.Length - arrayIndex)
				{
					throw new ArgumentNullException("Not enough space in array from arrayIndex to end of array");
				}
				int num = arrayIndex;
				using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator = GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<TKey, TValue> current = enumerator.Current;
						array[num++] = current;
					}
				}
			}
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			Add(keyValuePair.Key, keyValuePair.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = Find(keyValuePair.Key);
			if (num >= 0)
			{
				return Comparer<KeyValuePair<TKey, TValue>>.Default.Compare(table[num], keyValuePair) == 0;
			}
			return false;
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = Find(keyValuePair.Key);
			if (num >= 0 && Comparer<KeyValuePair<TKey, TValue>>.Default.Compare(table[num], keyValuePair) == 0)
			{
				RemoveAt(num);
				return true;
			}
			return false;
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			for (int i = 0; i < inUse; i++)
			{
				KeyValuePair<TKey, TValue> current = table[i];
				yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		void IDictionary.Add(object key, object value)
		{
			PutImpl(ToKey(key), ToValue(value), overwrite: false);
		}

		bool IDictionary.Contains(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException();
			}
			if (!(key is TKey))
			{
				return false;
			}
			return Find((TKey)key) >= 0;
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Enumerator(this, EnumeratorMode.ENTRY_MODE);
		}

		void IDictionary.Remove(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (key is TKey)
			{
				int num = IndexOfKey((TKey)key);
				if (num >= 0)
				{
					RemoveAt(num);
				}
			}
		}

		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			if (Count != 0)
			{
				if (array == null)
				{
					throw new ArgumentNullException();
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (array.Rank > 1)
				{
					throw new ArgumentException("array is multi-dimensional");
				}
				if (arrayIndex >= array.Length)
				{
					throw new ArgumentNullException("arrayIndex is greater than or equal to array.Length");
				}
				if (Count > array.Length - arrayIndex)
				{
					throw new ArgumentNullException("Not enough space in array from arrayIndex to end of array");
				}
				IEnumerator<KeyValuePair<TKey, TValue>> enumerator = GetEnumerator();
				int num = arrayIndex;
				while (enumerator.MoveNext())
				{
					array.SetValue(enumerator.Current, num++);
				}
			}
		}

		public void RemoveAt(int index)
		{
			KeyValuePair<TKey, TValue>[] array = table;
			int count = Count;
			if (index >= 0 && index < count)
			{
				if (index != count - 1)
				{
					Array.Copy(array, index + 1, array, index, count - 1 - index);
				}
				else
				{
					array[index] = default(KeyValuePair<TKey, TValue>);
				}
				inUse--;
				modificationCount++;
				return;
			}
			throw new ArgumentOutOfRangeException("index out of range");
		}

		public int IndexOfKey(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int num = 0;
			try
			{
				num = Find(key);
			}
			catch (Exception)
			{
				throw new InvalidOperationException();
			}
			return num | (num >> 31);
		}

		public int IndexOfValue(TValue value)
		{
			if (inUse == 0)
			{
				return -1;
			}
			for (int i = 0; i < inUse; i++)
			{
				KeyValuePair<TKey, TValue> keyValuePair = table[i];
				if (object.Equals(value, keyValuePair.Value))
				{
					return i;
				}
			}
			return -1;
		}

		public bool ContainsValue(TValue value)
		{
			return IndexOfValue(value) >= 0;
		}

		public void TrimExcess()
		{
			if ((double)inUse < (double)table.Length * 0.9)
			{
				Capacity = inUse;
			}
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int num = Find(key);
			if (num >= 0)
			{
				value = table[num].Value;
				return true;
			}
			value = default(TValue);
			return false;
		}

		private void EnsureCapacity(int n, int free)
		{
			KeyValuePair<TKey, TValue>[] array = table;
			KeyValuePair<TKey, TValue>[] array2 = null;
			int capacity = Capacity;
			bool flag = free >= 0 && free < Count;
			if (n > capacity)
			{
				UnityEngine.Debug.Log(string.Empty + n + " " + capacity);
				array2 = new KeyValuePair<TKey, TValue>[n << 1];
			}
			if (array2 != null)
			{
				if (flag)
				{
					if (free > 0)
					{
						Array.Copy(array, 0, array2, 0, free);
					}
					int num = Count - free;
					if (num > 0)
					{
						Array.Copy(array, free, array2, free + 1, num);
					}
				}
				else
				{
					Array.Copy(array, array2, Count);
				}
				table = array2;
			}
			else if (flag)
			{
				Array.Copy(array, free, array, free + 1, Count - free);
			}
		}

		private void PutImpl(TKey key, TValue value, bool overwrite)
		{
			if (key == null)
			{
				throw new ArgumentNullException("null key");
			}
			KeyValuePair<TKey, TValue>[] array = table;
			int num = -1;
			try
			{
				num = Find(key);
			}
			catch (Exception)
			{
				throw new InvalidOperationException();
			}
			if (num < 0)
			{
				num = ~num;
				if (num > Capacity + 1)
				{
					throw new Exception("SortedListNA::internal error (" + key + ", " + value + ") at [" + num + "]");
				}
				EnsureCapacity(Count + 1, num);
				array = table;
				array[num] = new KeyValuePair<TKey, TValue>(key, value);
				inUse++;
				modificationCount++;
			}
		}

		private void Init(IComparer<TKey> comparer, int capacity, bool forceSize)
		{
			if (comparer == null)
			{
				comparer = Comparer<TKey>.Default;
			}
			this.comparer = comparer;
			if (!forceSize && capacity < defaultCapacity)
			{
				capacity = defaultCapacity;
			}
			defaultCapacity = capacity;
			table = new KeyValuePair<TKey, TValue>[capacity];
			inUse = 0;
			modificationCount = 0;
		}

		private void CopyToArray(Array arr, int i, EnumeratorMode mode)
		{
			if (arr == null)
			{
				throw new ArgumentNullException("arr");
			}
			if (i < 0 || i + Count > arr.Length)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			IEnumerator enumerator = new Enumerator(this, mode);
			while (enumerator.MoveNext())
			{
				arr.SetValue(enumerator.Current, i++);
			}
		}

		private int Find(TKey key)
		{
			KeyValuePair<TKey, TValue>[] array = table;
			int count = Count;
			if (count == 0)
			{
				return -1;
			}
			int num = 0;
			int num2 = count - 1;
			while (num <= num2)
			{
				int num3 = num + num2 >> 1;
				int num4 = comparer.Compare(array[num3].Key, key);
				if (num4 == 0)
				{
					return num3;
				}
				if (num4 < 0)
				{
					num = num3 + 1;
				}
				else
				{
					num2 = num3 - 1;
				}
			}
			return ~num;
		}

		private TKey ToKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!(key is TKey))
			{
				throw new ArgumentException("The value \"" + key + "\" isn't of type \"" + typeof(TKey) + "\" and can't be used in this generic collection.", "key");
			}
			return (TKey)key;
		}

		private TValue ToValue(object value)
		{
			if (!(value is TValue))
			{
				throw new ArgumentException("The value \"" + value + "\" isn't of type \"" + typeof(TValue) + "\" and can't be used in this generic collection.", "value");
			}
			return (TValue)value;
		}

		public TKey KeyAt(int index)
		{
			if (index >= 0 && index < Count)
			{
				return table[index].Key;
			}
			throw new ArgumentOutOfRangeException("Index out of range");
		}

		public TValue ValueAt(int index)
		{
			if (index >= 0 && index < Count)
			{
				return table[index].Value;
			}
			throw new ArgumentOutOfRangeException("Index out of range");
		}
	}
}
