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

namespace Krach.Fourier
{
	public class SpectrumElement
	{
		readonly double frequency;
		readonly double amplitude;
		readonly double phase;

		public double Frequency { get { return frequency; } }
		public double Amplitude { get { return amplitude; } }
		public double Phase { get { return phase; } }

		public SpectrumElement(double frequency, double amplitude, double phase)
		{
			if (frequency < 0) throw new ArgumentOutOfRangeException("frequency");

			this.frequency = frequency;
			this.amplitude = amplitude;
			this.phase = phase;
		}

		public override string ToString()
		{
			return string.Format("Frequency: {0}, Amplitude: {1}, Phase: {2}", frequency, amplitude, phase);
		}
	}
}
