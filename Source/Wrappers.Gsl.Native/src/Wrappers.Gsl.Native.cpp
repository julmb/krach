#include <gsl/gsl_wavelet.h>

extern "C"
{
	const gsl_wavelet_type* GetWaveletTypeDaubechies()
	{
		return gsl_wavelet_daubechies;
	}
	const gsl_wavelet_type* GetWaveletTypeDaubechiesCentered()
	{
		return gsl_wavelet_daubechies_centered;
	}
	const gsl_wavelet_type* GetWaveletTypeHaar()
	{
		return gsl_wavelet_haar;
	}
	const gsl_wavelet_type* GetWaveletTypeHaarCentered()
	{
		return gsl_wavelet_haar_centered;
	}
	const gsl_wavelet_type* GetWaveletTypeBSpline()
	{
		return gsl_wavelet_bspline;
	}
	const gsl_wavelet_type* GetWaveletTypeBSplineCentered()
	{
		return gsl_wavelet_bspline_centered;
	}

	gsl_wavelet* CreateWavelet(gsl_wavelet_type* type, size_t k)
	{
		return gsl_wavelet_alloc(type, k);
	}
	void DisposeWavelet(gsl_wavelet* wavelet)
	{
		return gsl_wavelet_free(wavelet);
	}

	gsl_wavelet_workspace* CreateWaveletWorkspace(size_t elementCount)
	{
		return gsl_wavelet_workspace_alloc(elementCount);
	}
	void DisposeWaveletWorkspace(gsl_wavelet_workspace* waveletWorkspace)
	{
		return gsl_wavelet_workspace_free(waveletWorkspace);
	}

	void WaveletTransformForward(gsl_wavelet* wavelet, gsl_wavelet_workspace* waveletWorkspace, double* data, size_t elementCount, size_t stride)
	{
		gsl_wavelet_transform_forward(wavelet, data, stride, elementCount, waveletWorkspace);
	}
	void WaveletTransformReverse(gsl_wavelet* wavelet, gsl_wavelet_workspace* waveletWorkspace, double* data, size_t elementCount, size_t stride)
	{
		gsl_wavelet_transform_inverse(wavelet, data, stride, elementCount, waveletWorkspace);
	}
}
