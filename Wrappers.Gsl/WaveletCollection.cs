using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Wrappers.Gsl
{
	public class WaveletCollection
	{
		readonly double smoothingCoefficient;
		readonly IEnumerable<IEnumerable<double>> decomposition;

		public double SmoothingCoefficient { get { return smoothingCoefficient; } }
		public IEnumerable<IEnumerable<double>> Decomposition { get { return decomposition; } }
		public IEnumerable<Wavelet> Wavelets
		{
			get
			{
				return
				(
					from level in decomposition
					from itemIndex in Enumerable.Range(0, level.Count())
					let scale = 1.0 / level.Count()
					let translation = (double)itemIndex / (double)level.Count()
					let item = level.ElementAt(itemIndex)
					select new Wavelet(scale, translation, item)
				)
				.ToArray();					
			}
		}

		public WaveletCollection(double smoothingCoefficient, IEnumerable<IEnumerable<double>> decomposition)
		{
			if (decomposition == null) throw new ArgumentNullException("levels");
			for (int levelIndex = 0; levelIndex < decomposition.Count(); levelIndex++)
				if (decomposition.ElementAt(levelIndex).Count() != (int)Scalars.Exponentiate(2, levelIndex))
					throw new ArgumentException("parameter 'decomposition' doesn't have the correct format");

			this.smoothingCoefficient = smoothingCoefficient;
			this.decomposition = decomposition;
		}

		public static WaveletCollection FromWaveletData(IEnumerable<double> waveletData)
		{
			int index = 0;

			double smoothingCoefficient = waveletData.ElementAt(index++);

			List<List<double>> decomposition = new List<List<double>>();

			int levelCount = (int)Scalars.Logarithm(2, waveletData.Count());

			for (int levelIndex = 0; levelIndex < levelCount; levelIndex++)
			{
				List<double> level = new List<double>();

				int itemCount = (int)Scalars.Exponentiate(2, levelIndex);

				for (int itemIndex = 0; itemIndex < itemCount; itemIndex++) level.Add(waveletData.ElementAt(index++));

				decomposition.Add(level);
			}

			return new WaveletCollection(smoothingCoefficient, decomposition);
		}
		public static IEnumerable<double> ToWaveletData(WaveletCollection waveletCollection)
		{
			return Enumerables.Concatenate
			(
				Enumerables.Create(waveletCollection.smoothingCoefficient),
				from level in waveletCollection.decomposition
				from item in level
				select item
			)
			.ToArray();
		}
	}
}

