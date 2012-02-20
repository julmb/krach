using System;

namespace Krach.Basics
{
	public class ProbabilityEntry<TKey>
	{
		readonly TKey key;
		readonly double probability;

		public TKey Key { get { return key; } }
		public double Probability { get { return probability; } }

		public ProbabilityEntry(TKey key, double probability)
		{
			if (probability < 0 || probability > 1) throw new ArgumentOutOfRangeException("probability");

			this.key = key;
			this.probability = probability;
		}
	}
}
