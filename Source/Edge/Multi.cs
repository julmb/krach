using System;
using System.Collections.Generic;

namespace Utilities
{
	public static class Multi
	{
		/// <summary>
		/// Enumerates over multiple collections at once, and breaks once one of them ends.
		/// </summary>
		/// <typeparam name="T1">The type of the first collection.</typeparam>
		/// <typeparam name="T2">The type of the second collection.</typeparam>
		/// <param name="source1">The first collection.</param>
		/// <param name="source2">The second collection.</param>
		/// <returns>The enumerable of the collections.</returns>
		public static IEnumerable<IMulti<T1, T2>> Foreach<T1, T2>(IEnumerable<T1> source1, IEnumerable<T2> source2)
		{
			if (source1 == null) throw new ArgumentNullException("source1");
			if (source2 == null) throw new ArgumentNullException("source2");

			IEnumerator<T1> enumerator1 = source1.GetEnumerator();
			IEnumerator<T2> enumerator2 = source2.GetEnumerator();

			while (enumerator1.MoveNext() && enumerator2.MoveNext())
				yield return new Multi<T1, T2>(enumerator1.Current, enumerator2.Current);
		}
		/// <summary>
		/// Enumerates over multiple collections at once, and breaks once one of them ends.
		/// </summary>
		/// <typeparam name="T1">The type of the first collection.</typeparam>
		/// <typeparam name="T2">The type of the second collection.</typeparam>
		/// <typeparam name="T3">The type of the third collection.</typeparam>
		/// <param name="source1">The first collection.</param>
		/// <param name="source2">The second collection.</param>
		/// <param name="source3">The third collection.</param>
		/// <returns>The enumerable of the collections.</returns>
		public static IEnumerable<IMulti<T1, T2, T3>> Foreach<T1, T2, T3>(IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3)
		{
			if (source1 == null) throw new ArgumentNullException("source1");
			if (source2 == null) throw new ArgumentNullException("source2");
			if (source3 == null) throw new ArgumentNullException("source3");

			IEnumerator<T1> enumerator1 = source1.GetEnumerator();
			IEnumerator<T2> enumerator2 = source2.GetEnumerator();
			IEnumerator<T3> enumerator3 = source3.GetEnumerator();

			while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext())
				yield return new Multi<T1, T2, T3>(enumerator1.Current, enumerator2.Current, enumerator3.Current);
		}
		/// <summary>
		/// Enumerates over multiple collections at once, and breaks once one of them ends.
		/// </summary>
		/// <typeparam name="T1">The type of the first collection.</typeparam>
		/// <typeparam name="T2">The type of the second collection.</typeparam>
		/// <typeparam name="T3">The type of the third collection.</typeparam>
		/// <typeparam name="T4">The type of the fourth collection.</typeparam>
		/// <param name="source1">The first collection.</param>
		/// <param name="source2">The second collection.</param>
		/// <param name="source3">The third collection.</param>
		/// <param name="source4">The fourth collection.</param>
		/// <returns>The enumerable of the collections.</returns>
		public static IEnumerable<IMulti<T1, T2, T3, T4>> Foreach<T1, T2, T3, T4>(IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3, IEnumerable<T4> source4)
		{
			if (source1 == null) throw new ArgumentNullException("source1");
			if (source2 == null) throw new ArgumentNullException("source2");
			if (source3 == null) throw new ArgumentNullException("source3");
			if (source4 == null) throw new ArgumentNullException("source4");

			IEnumerator<T1> enumerator1 = source1.GetEnumerator();
			IEnumerator<T2> enumerator2 = source2.GetEnumerator();
			IEnumerator<T3> enumerator3 = source3.GetEnumerator();
			IEnumerator<T4> enumerator4 = source4.GetEnumerator();

			while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext() && enumerator4.MoveNext())
				yield return new Multi<T1, T2, T3, T4>(enumerator1.Current, enumerator2.Current, enumerator3.Current, enumerator4.Current);
		}
		/// <summary>
		/// Enumerates over multiple collections at once, and breaks once one of them ends.
		/// </summary>
		/// <typeparam name="T1">The type of the first collection.</typeparam>
		/// <typeparam name="T2">The type of the second collection.</typeparam>
		/// <typeparam name="T3">The type of the third collection.</typeparam>
		/// <typeparam name="T4">The type of the fourth collection.</typeparam>
		/// <typeparam name="T5">The type of the fifth collection.</typeparam>
		/// <param name="source1">The first collection.</param>
		/// <param name="source2">The second collection.</param>
		/// <param name="source3">The third collection.</param>
		/// <param name="source4">The fourth collection.</param>
		/// <param name="source5">The fifth collection.</param>
		/// <returns>The enumerable of the collections.</returns>
		public static IEnumerable<IMulti<T1, T2, T3, T4, T5>> Foreach<T1, T2, T3, T4, T5>(IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3, IEnumerable<T4> source4, IEnumerable<T5> source5)
		{
			if (source1 == null) throw new ArgumentNullException("source1");
			if (source2 == null) throw new ArgumentNullException("source2");
			if (source3 == null) throw new ArgumentNullException("source3");
			if (source4 == null) throw new ArgumentNullException("source4");
			if (source5 == null) throw new ArgumentNullException("source5");

			IEnumerator<T1> enumerator1 = source1.GetEnumerator();
			IEnumerator<T2> enumerator2 = source2.GetEnumerator();
			IEnumerator<T3> enumerator3 = source3.GetEnumerator();
			IEnumerator<T4> enumerator4 = source4.GetEnumerator();
			IEnumerator<T5> enumerator5 = source5.GetEnumerator();

			while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext() && enumerator4.MoveNext() && enumerator5.MoveNext())
				yield return new Multi<T1, T2, T3, T4, T5>(enumerator1.Current, enumerator2.Current, enumerator3.Current, enumerator4.Current, enumerator5.Current);
		}
	}
	class Multi<T1, T2> : IMulti<T1, T2>
	{
		readonly T1 data1;
		readonly T2 data2;

		public T1 Data1 { get { return data1; } }
		public T2 Data2 { get { return data2; } }

		public Multi(T1 data1, T2 data2)
		{
			this.data1 = data1;
			this.data2 = data2;
		}
	}
	class Multi<T1, T2, T3> : IMulti<T1, T2, T3>
	{
		readonly T1 data1;
		readonly T2 data2;
		readonly T3 data3;

		public T1 Data1 { get { return data1; } }
		public T2 Data2 { get { return data2; } }
		public T3 Data3 { get { return data3; } }

		public Multi(T1 data1, T2 data2, T3 data3)
		{
			this.data1 = data1;
			this.data2 = data2;
			this.data3 = data3;
		}
	}
	class Multi<T1, T2, T3, T4> : IMulti<T1, T2, T3, T4>
	{
		readonly T1 data1;
		readonly T2 data2;
		readonly T3 data3;
		readonly T4 data4;

		public T1 Data1 { get { return data1; } }
		public T2 Data2 { get { return data2; } }
		public T3 Data3 { get { return data3; } }
		public T4 Data4 { get { return data4; } }

		public Multi(T1 data1, T2 data2, T3 data3, T4 data4)
		{
			this.data1 = data1;
			this.data2 = data2;
			this.data3 = data3;
			this.data4 = data4;
		}
	}
	class Multi<T1, T2, T3, T4, T5> : IMulti<T1, T2, T3, T4, T5>
	{
		readonly T1 data1;
		readonly T2 data2;
		readonly T3 data3;
		readonly T4 data4;
		readonly T5 data5;

		public T1 Data1 { get { return data1; } }
		public T2 Data2 { get { return data2; } }
		public T3 Data3 { get { return data3; } }
		public T4 Data4 { get { return data4; } }
		public T5 Data5 { get { return data5; } }

		public Multi(T1 data1, T2 data2, T3 data3, T4 data4, T5 data5)
		{
			this.data1 = data1;
			this.data2 = data2;
			this.data3 = data3;
			this.data4 = data4;
			this.data5 = data5;
		}
	}
}
