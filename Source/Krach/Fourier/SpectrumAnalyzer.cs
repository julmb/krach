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

using System.Collections.Generic;
using System.Linq;
using Krach.Basics;
using Krach.Extensions;

namespace Krach.Fourier
{
	public class SpectrumAnalyzer
	{
		readonly List<SpectrumElement> elements = new List<SpectrumElement>();

		public IEnumerable<SpectrumElement> Elements { get { return elements; } }

		public SpectrumAnalyzer(IEnumerable<double> signal, double length)
		{
			Complex[] spectrum = DiscreteFourierTransform.TransformForward(signal.Select(sample => (Complex)sample)).ToArray();

			for (int index = 0; index < spectrum.Length; index++)
			{
				double frequency = index / length;
				double cosineAmplitude = spectrum[index].Real;
				double sineAmplitude = spectrum[index].Imaginary;
				double amplitude = Scalars.SquareRoot(cosineAmplitude.Square() + sineAmplitude.Square());
				double phase = Scalars.ArcTangent(sineAmplitude, cosineAmplitude);

				elements.Add(new SpectrumElement(frequency, amplitude, phase));
			}
		}
	}
}
