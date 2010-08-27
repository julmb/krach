using System.Collections.Generic;

namespace Krach.MachineLearning
{
	public interface IGameState
	{
		IEnumerable<double> ToPosition();
	}
}
