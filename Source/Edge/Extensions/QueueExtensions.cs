using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Extensions
{
	public static class QueueExtensions
	{
		public static IEnumerable<T> Dequeue<T>(this Queue<T> source, Func<T, bool> predicate)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (predicate == null) throw new ArgumentNullException("predicate");

			List<T> result = new List<T>();

			while (source.Any())
			{
				if (!predicate(source.Peek())) break;

				result.Add(source.Dequeue());
			}

			return result;
		}
		public static IEnumerable<T> Dequeue<T>(this Queue<T> source, int count)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (count > source.Count) throw new ArgumentOutOfRangeException("count");

			List<T> result = new List<T>();
			
			for (int i = 0; i < count; i++) result.Add(source.Dequeue());

			return result;
		}
	}
}
