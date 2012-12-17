using System;
using System.Linq;
using Krach.Basics;
using Krach.Extensions;
using System.Collections.Generic;

namespace Krach.Analysis
{
	public class MatrixAssignment : Assignment
	{
		readonly IEnumerable<Variable> variables;
		readonly Matrix matrix;

		public MatrixAssignment(IEnumerable<Variable> variables, Matrix matrix)
		{
			if (variables == null) throw new ArgumentNullException("variables");

			this.variables = variables;
			this.matrix = matrix;
		}

		public override double GetValue(Variable variable)
		{
			return matrix[variables.GetIndex(variable), 0];
		}
	}
}

