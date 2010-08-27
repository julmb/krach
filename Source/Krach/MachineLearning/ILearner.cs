using System.Collections.Generic;

namespace Krach.MachineLearning
{
	public interface ILearner
	{
		double this[IEnumerable<double> position] { get; set; }
	}
}
