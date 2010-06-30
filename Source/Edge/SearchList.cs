// Copyright © Julian Brunner 2009 - 2010

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Stream Visualizer.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
	public class SearchList<TValue, TKey> : IEnumerable<TValue>
	{
		readonly Func<TValue, TKey> keySelector;
		readonly IComparer<TKey> comparer;
		readonly List<TValue> items;

		public TValue this[int index]
		{
			get
			{
				if (index < 0 || index >= items.Count) throw new ArgumentOutOfRangeException("index");

				return items[index];
			}
		}
		public TValue this[TKey key]
		{
			get
			{
				int index = FindIndex(key);

				if (index < 0 || index >= items.Count) throw new KeyNotFoundException();

				return items[index];
			}
		}
		public TValue[] this[int startIndex, int endIndex]
		{
			get
			{
				if (startIndex < 0 || startIndex > items.Count) throw new ArgumentOutOfRangeException("startIndex");
				if (endIndex < 0 || endIndex > items.Count) throw new ArgumentOutOfRangeException("endIndex");
				if (endIndex - startIndex < 0) throw new ArgumentException();

				TValue[] buffer = new TValue[endIndex - startIndex];
				items.CopyTo(startIndex, buffer, 0, buffer.Length);

				return buffer;
			}
		}
		public TValue[] this[TKey startKey, TKey endKey]
		{
			get
			{
				int startIndex = FindIndex(startKey);
				int endIndex = FindIndex(endKey);

				if (startIndex == endIndex) return new TValue[0];

				return this[startIndex, endIndex];
			}
		}
		public TValue[] this[Range<TKey> range] { get { return this[range.Start, range.End]; } }

		public bool IsEmpty { get { return items.Count == 0; } }
		public int Count { get { return items.Count; } }

		public SearchList(Func<TValue, TKey> keySelector, IComparer<TKey> comparer)
		{
			if (keySelector == null) throw new ArgumentNullException("keySelector");
			if (comparer == null) throw new ArgumentNullException("comparer");

			this.keySelector = keySelector;
			this.comparer = comparer;
			this.items = new List<TValue>();
		}
		public SearchList(Func<TValue, TKey> keySelector) : this(keySelector, Comparer<TKey>.Default) { }

		public void Clear()
		{
			items.Clear();
		}
		public void Append(TValue item)
		{
			if (items.Count > 0 && comparer.Compare(keySelector(item), keySelector(items[items.Count - 1])) < 0) throw new ArgumentException("Item cannot be appended without violating list ordering.");

			items.Add(item);
		}
		public void Append(IEnumerable<TValue> items)
		{
			foreach (TValue item in items) Append(item);
		}
		public void Insert(TValue item)
		{
			items.Insert(FindIndex(keySelector(item)), item);
		}
		public void Insert(IEnumerable<TValue> items)
		{
			foreach (TValue item in items) Insert(item);
		}
		public void Remove(TValue item)
		{
			items.Remove(item);
		}
		public void Remove(IEnumerable<TValue> items)
		{
			foreach (TValue item in items) Remove(item);
		}
		public void Remove(int startIndex, int endIndex)
		{
			items.RemoveRange(startIndex, endIndex - startIndex);
		}
		public void Remove(int index)
		{
			items.RemoveAt(index);
		}
		/// <summary>
		/// Returns the index of the first item which has a key that is greater than or equal to <paramref name="key"/>.
		/// If no such item is found, Count is returned.
		/// </summary>
		/// <param name="key">The key to search for.</param>
		/// <returns>The index of the first item which has a key that is greater than or equal to <paramref name="key"/>.</returns>
		public int FindIndex(TKey key)
		{
			int startIndex = 0;
			int endIndex = items.Count;

			while (startIndex != endIndex)
			{
				int index = (startIndex + endIndex) / 2;

				if (comparer.Compare(keySelector(items[index]), key) >= 0) endIndex = index;
				else startIndex = index + 1;
			}

			return startIndex;
		}
		public IEnumerator<TValue> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}