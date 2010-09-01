// Copyright © Julian Brunner 2010

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach
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
			return Scalars.Exponentiate(NextDouble(Scalars.Logarithm(minimum), Scalars.Logarithm(maximum)));
		}
		public double NextExponential(double minimum, double maximum)
		{
			return Scalars.Logarithm(NextDouble(Scalars.Exponentiate(minimum), Scalars.Exponentiate(maximum)));
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

		protected virtual double NextSample()
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
