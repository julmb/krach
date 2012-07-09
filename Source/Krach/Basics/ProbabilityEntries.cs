using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Basics
{
	public class ProbabilityEntries<TKey> : IEnumerable<ProbabilityEntry<TKey>>
	{
		readonly IEnumerable<ProbabilityEntry<TKey>> entries;

		public double this[TKey key]
		{
			get
			{
				foreach (ProbabilityEntry<TKey> entry in entries)
					if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
						return entry.Probability;

				return 0;
			}
		}

		public ProbabilityEntries(IEnumerable<ProbabilityEntry<TKey>> entries)
		{
			if (entries == null) throw new ArgumentNullException("entries");
			double sum = entries.Sum(entry => entry.Probability).Round(0.0001);
			if (sum != 1.0) throw new ArgumentException(String.Format("The entries in parameter 'entries' do not sum up to 1, the sum is {0}.", sum));

			this.entries = entries;
		}

		public IEnumerator<ProbabilityEntry<TKey>> GetEnumerator()
		{
			return entries.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
