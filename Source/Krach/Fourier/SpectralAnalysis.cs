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
using Krach.Basics;
using Krach.Extensions;

namespace Krach.Fourier
{
	public static class SpectralAnalysis
	{
		public static IEnumerable<SpectralElement> SignalToSpectrum(IEnumerable<double> signal, double length)
		{
			Complex[] spectrum = DiscreteFourierTransform.TransformForward(signal.Select(sample => (Complex)sample)).ToArray();

			for (int index = 0; index < spectrum.Length; index++)
			{
				double frequencyIndex = index > spectrum.Length / 2 ? index - spectrum.Length : index;
				double frequency = frequencyIndex / length;
				double cosineAmplitude = spectrum[index].Real;
				double sineAmplitude = spectrum[index].Imaginary;
				double mixedAmplitude = Scalars.SquareRoot(cosineAmplitude.Square() + sineAmplitude.Square());
				double amplitude = mixedAmplitude / Scalars.SquareRoot(spectrum.Length);
				double phase = Scalars.ArcTangent(sineAmplitude, cosineAmplitude);

				yield return new SpectralElement(frequency, amplitude, phase);
			}
		}
		public static IEnumerable<double> SpectrumToSignal(IEnumerable<SpectralElement> elements, double length, int sampleCount)
		{
			double[] signal = new double[sampleCount];

			foreach (SpectralElement element in elements)
				for (int sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
				{
					double amplitude = element.Amplitude;
					double timeFraction = sampleIndex * length / sampleCount;
					double phaseFraction = 2 * Math.PI * timeFraction;
					double phase = element.Frequency * phaseFraction - element.Phase;

					signal[sampleIndex] += amplitude * Scalars.Cosine(phase);
				}

			return signal;
		}
	}
}
