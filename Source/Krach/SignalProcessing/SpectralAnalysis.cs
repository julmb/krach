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
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Basics;
using Krach.Extensions;

namespace Krach.SignalProcessing
{
	public static class SpectralAnalysis
	{
		public static IEnumerable<Wave> SignalToSpectrum(IEnumerable<double> signal, double length)
		{
			Complex[] spectrum = DiscreteFourierTransform.TransformForward(signal.Select(sample => (Complex)sample)).ToArray();

			for (int spectrumIndex = 0; spectrumIndex < spectrum.Length; spectrumIndex++)
			{
				double frequencyIndex = spectrumIndex > spectrum.Length / 2 ? spectrumIndex - spectrum.Length : spectrumIndex;
				double frequency = frequencyIndex / length;
				double cosineAmplitude = spectrum[spectrumIndex].Real;
				double sineAmplitude = spectrum[spectrumIndex].Imaginary;
				double mixedAmplitude = Scalars.SquareRoot(cosineAmplitude.Square() + sineAmplitude.Square());
				double amplitude = mixedAmplitude / Scalars.SquareRoot(spectrum.Length);
				double phase = Scalars.ArcTangent(sineAmplitude, cosineAmplitude);

				yield return new Wave(frequency, amplitude, -phase / (2 * Math.PI) + 0.25);
			}
		}
		public static IEnumerable<double> SpectrumToSignal(IEnumerable<Wave> elements, double length, int sampleCount)
		{
			double[] signal = new double[sampleCount];

			foreach (Wave wave in elements)
				for (int sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
					signal[sampleIndex] += wave.GetValue(sampleIndex * length / sampleCount);

			return signal;
		}
		public static IEnumerable<Wave> Simplify(IEnumerable<Wave> elements)
		{
			Dictionary<double, Wave> result = new Dictionary<double, Wave>();

			foreach (Wave element in elements)
			{
				double frequency = element.Frequency.Absolute();

				if (!result.ContainsKey(frequency)) result.Add(frequency, new Wave(frequency, 0, 0));

				result[frequency] += element;
			}

			return result.Values;
		}
	}
}
