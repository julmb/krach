using System;
using System.Collections.Generic;

namespace Krach.MachineLearning
{
	public class SvmLearner : ILearner
	{
		readonly int dimensions;

		public double this[IEnumerable<double> position]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public SvmLearner(int dimensions)
		{
			if (dimensions < 0) throw new ArgumentOutOfRangeException("dimensions");

			this.dimensions = dimensions;
		}
	}
}
