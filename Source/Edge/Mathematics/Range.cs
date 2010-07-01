using System;
using System.Collections.Generic;

namespace Edge.Mathematics
{
	public struct Range<T> where T : IComparable<T>
	{
		readonly T start;
		readonly T end;

		public T Start { get { return start; } }
		public T End { get { return end; } }
		public bool IsEmpty { get { return Comparer<T>.Default.Compare(start, end) >= 0; } }

		public Range(T start, T end)
		{
			this.start = start;
			this.end = end;
		}

		public override string ToString()
		{
			return start + " - " + end;
		}
	}
}
