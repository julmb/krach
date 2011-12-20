using System;

namespace Krach.Basics
{
	public class ProbabilityEntry<TItem>
	{
		readonly TItem item;
		readonly double probability;

		public TItem Item { get { return item; } }
		public double Probability { get { return probability; } }

		public ProbabilityEntry(TItem item, double probability)
		{
			if (probability < 0 || probability > 1) throw new ArgumentOutOfRangeException("probability");

			this.item = item;
			this.probability = probability;
		}
	}
}
