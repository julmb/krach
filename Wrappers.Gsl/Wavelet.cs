using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Wrappers.Gsl
{
	public class Wavelet
	{
		readonly double scale;
		readonly double translation;
		readonly double amplitude;

		public double Scale { get { return scale; } }
		public double Translation { get { return translation; } }
		public double Amplitude { get { return amplitude; } }

		public Wavelet(double scale, double translation, double amplitude)
		{
			if (scale <= 0 || scale > 1) throw new ArgumentOutOfRangeException("scale");
			if (translation < 0 || translation >= 1) throw new ArgumentOutOfRangeException("translation");

			this.scale = scale;
			this.translation = translation;
			this.amplitude = amplitude;
		}
	}
}

