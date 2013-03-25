using System;
using System.Collections.Generic;

namespace Krach.Calculus
{
	class ExplicitValue : IValue
	{
		readonly int dimension;
		readonly IEnumerable<double> value;
		
		public int Dimension { get { return dimension; } }
		
		public ExplicitValue(int dimension, IEnumerable<double> value)
		{
			if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
			if (value == null) throw new ArgumentNullException("value");
			
			this.dimension = dimension;
			this.value = value;
		}
		
		public IEnumerable<double> Evaluate()
		{
			return value;
		}
	}
}

