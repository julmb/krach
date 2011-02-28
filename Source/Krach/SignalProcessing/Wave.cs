// Copyright © Julian Brunner 2010 - 2011

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
using Krach.Extensions;

namespace Krach.SignalProcessing
{
	public class Wave
	{
		readonly double frequency;
		readonly double amplitude;
		readonly double phase;

		public double Frequency { get { return frequency; } }
		public double Amplitude { get { return amplitude; } }
		public double Phase { get { return phase; } }

		public Wave(double frequency, double amplitude, double phase)
		{
			this.frequency = frequency.Absolute();
			this.amplitude = amplitude;
			this.phase = frequency < 0 ? 0.5 - phase : phase;
		}

		public override string ToString()
		{
			return string.Format("Frequency: {0}, Amplitude: {1}, Phase: {2}", frequency, amplitude, phase);
		}
		public double GetValue(double time)
		{
			return amplitude * Scalars.PSine(frequency * time + phase);
		}

		public static Wave operator +(Wave wave1, Wave wave2)
		{
			if (wave1.frequency != wave2.frequency) throw new ArgumentException("The frequencies don't match.");

			double basePhase = wave1.phase;
			double phaseDifference = wave2.phase - wave1.phase;

			double frequency = Items.Equal(wave1.frequency, wave2.frequency);
			double amplitude = Scalars.SquareRoot
			(
				wave1.amplitude.Square() +
				2 * wave1.amplitude * wave2.amplitude * Scalars.PSine(phaseDifference + 0.25) +
				wave2.amplitude.Square()
			);
			double phase =
				basePhase +
				Scalars.ArcTangent
				(
					wave2.amplitude * Scalars.PSine(phaseDifference),
					wave1.amplitude + wave2.amplitude * Scalars.PSine(phaseDifference + 0.25)
				)
				/ (2 * Math.PI);

			return new Wave(frequency, amplitude, phase);
		}
	}
}
