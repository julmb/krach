using System;
using System.Runtime.InteropServices;

namespace Wrappers.Gsl
{
	static class Native
	{
		[DllImport("Wrappers.Gsl.Native")]
		public static extern IntPtr GetWaveletTypeDaubechies();
		[DllImport("Wrappers.Gsl.Native")]
		public static extern IntPtr GetWaveletTypeDaubechiesCentered();
		[DllImport("Wrappers.Gsl.Native")]
		public static extern IntPtr GetWaveletTypeHaar();
		[DllImport("Wrappers.Gsl.Native")]
		public static extern IntPtr GetWaveletTypeHaarCentered();
		[DllImport("Wrappers.Gsl.Native")]
		public static extern IntPtr GetWaveletTypeBSpline();
		[DllImport("Wrappers.Gsl.Native")]
		public static extern IntPtr GetWaveletTypeBSplineCentered();

		[DllImport("Wrappers.Gsl.Native")]
		public static extern IntPtr CreateWavelet(IntPtr waveletType, uint k);
		[DllImport("Wrappers.Gsl.Native")]
		public static extern void DisposeWavelet(IntPtr wavelet);

		[DllImport("Wrappers.Gsl.Native")]
		public static extern IntPtr CreateWaveletWorkspace(uint elementCount);
		[DllImport("Wrappers.Gsl.Native")]
		public static extern void DisposeWaveletWorkspace(IntPtr waveletWorkspace);

		[DllImport("Wrappers.Gsl.Native")]
		public static extern void WaveletTransformForward(IntPtr wavelet, IntPtr waveletWorkspace, IntPtr data, uint elementCount, uint stride);
		[DllImport("Wrappers.Gsl.Native")]
		public static extern void WaveletTransformReverse(IntPtr wavelet, IntPtr waveletWorkspace, IntPtr data, uint elementCount, uint stride);
	}
}

