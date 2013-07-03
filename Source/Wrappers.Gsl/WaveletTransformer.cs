using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Wrappers.Gsl
{
	public class WaveletTransformer : IDisposable
	{
		readonly IntPtr wavelet;

		bool disposed = false;

		public WaveletTransformer(WaveletType waveletType, int waveletParameter)
		{
			this.wavelet = Native.CreateWavelet(GetNativeWaveletType(waveletType), (uint)waveletParameter);
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;

				Native.DisposeWavelet(wavelet);
			}
		}

		public IEnumerable<double> TransformForward(IEnumerable<double> signal)
		{
			if (!Scalars.IsPowerOf2(signal.Count())) throw new ArgumentOutOfRangeException("signal");

			IntPtr waveletWorkspace = Native.CreateWaveletWorkspace((uint)signal.Count());

			IntPtr dataPointer = signal.Copy();

			Native.WaveletTransformForward(wavelet, waveletWorkspace, dataPointer, (uint)signal.Count(), 1);

			IEnumerable<double> waveletData = dataPointer.Read<double>(signal.Count());

			dataPointer.Free();

			Native.DisposeWaveletWorkspace(waveletWorkspace);

			return waveletData;
		}
		public IEnumerable<double> TransformReverse(IEnumerable<double> waveletData)
		{
			if (!Scalars.IsPowerOf2(waveletData.Count())) throw new ArgumentOutOfRangeException("waveletData");

			IntPtr waveletWorkspace = Native.CreateWaveletWorkspace((uint)waveletData.Count());

			IntPtr dataPointer = waveletData.Copy();

			Native.WaveletTransformReverse(wavelet, waveletWorkspace, dataPointer, (uint)waveletData.Count(), 1);

			IEnumerable<double> signal = dataPointer.Read<double>(waveletData.Count());

			dataPointer.Free();

			Native.DisposeWaveletWorkspace(waveletWorkspace);

			return signal;
		}

		static IntPtr GetNativeWaveletType(WaveletType waveletType)
		{
			switch (waveletType)
			{
				case WaveletType.Haar: return Native.GetWaveletTypeHaar();
				case WaveletType.Daubechies: return Native.GetWaveletTypeDaubechies();
				case WaveletType.BSpline: return Native.GetWaveletTypeBSpline();
				default: throw new ArgumentException("parameter 'waveletType' is not a valid wavelet type");
			}
		}
	}
}

