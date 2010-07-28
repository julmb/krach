using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Extensions;

namespace Dash
{
	public class RandomNumberGenerator
	{
		readonly Random random;

		public RandomNumberGenerator()
		{
			this.random = new Random();
		}
		public RandomNumberGenerator(int seed)
		{
			this.random = new Random(seed);
		}

		public int NextInteger(int minimum, int maximum)
		{
			return (int)NextDouble(minimum, maximum).Floor();
		}
		public int NextInteger(int maximum)
		{
			return NextInteger(0, maximum);
		}
		public double NextDouble(double minimum, double maximum)
		{
			return NextSample() * (maximum - minimum) + minimum;
		}
		public double NextDouble(double maximum)
		{
			return NextDouble(0, maximum);
		}
		public double NextDouble()
		{
			return NextDouble(0, 1);
		}
		public bool NextBool(double probability)
		{
			return NextDouble() < probability;
		}
		public bool NextBool()
		{
			return NextBool(0.5);
		}
		public double NextLogarithmic(double minimum, double maximum)
		{
			return Scalars.Exponentiate(NextDouble(minimum.Logarithm(), maximum.Logarithm()));
		}
		public double NextExponential(double minimum, double maximum)
		{
			return NextDouble(Scalars.Exponentiate(minimum), Scalars.Exponentiate(maximum)).Logarithm();
		}
		public int NextIndex(IEnumerable<double> probabilities)
		{
			double value = NextDouble();

			return Normalize(probabilities).TakeWhile(probability => (value -= probability) > 0).Count();
		}
		public T NextItem<T>(IEnumerable<T> source)
		{
			T[] sourceArray = source.ToArray();

			return sourceArray[NextInteger(sourceArray.Length)];
		}
		public T NextItem<T>(params T[] items)
		{
			return NextItem((IEnumerable<T>)items);
		}
		public IEnumerable<T> Shuffle<T>(IEnumerable<T> source)
		{
			List<T> sourceList = new List<T>(source);
			List<T> result = new List<T>();

			while (sourceList.Any())
			{
				int index = NextInteger(sourceList.Count);

				result.Add(sourceList[index]);
				sourceList.RemoveAt(index);
			}

			return result;
		}

		double NextSample()
		{
			return random.NextDouble();
		}

		static IEnumerable<double> Normalize(IEnumerable<double> probabilities)
		{
			double sum = probabilities.Sum();

			return probabilities.Select(probability => probability / sum);
		}
	}
}
