using System;
using System.Runtime.InteropServices;

namespace Wrappers.Gsl
{
	static class Native
	{
		[DllImport("gsl")]
		public static extern void gsl_wavelet_alloc(uint k);
	}
}

