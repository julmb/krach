using System;
using System.Linq;
using Krach.Basics;
using System.Collections.Generic;

namespace Krach.Analysis
{
	public class SymbolicFunction : Function
	{
		readonly IEnumerable<Variable> variables;
		readonly IEnumerable<Term> terms;

		public override int DomainDimension { get { return variables.Count(); } }
		public override int CodomainDimension { get { return terms.Count(); } }

		public SymbolicFunction(IEnumerable<Variable> variables, IEnumerable<Term> terms)
		{
			if (variables == null) throw new ArgumentNullException("variables");
			if (terms == null) throw new ArgumentNullException("terms");

			this.variables = variables;
			this.terms = terms;
		}

		public override IEnumerable<Matrix> GetValues(Matrix position)
		{
			Assignment assignment = new MatrixAssignment(variables, position);

			return
				from term in terms
				let derivative0 = term
				select Matrix.CreateSingleton(derivative0.Evaluate(assignment));
		}
		public override IEnumerable<Matrix> GetGradients(Matrix position)
		{
			Assignment assignment = new MatrixAssignment(variables, position);

			return
				from term in terms
				let derivative0 = term
				select Matrix.FromRowVectors
				(
					from variable1 in variables
					let derivative1 = derivative0.GetDerivative(variable1)
					select Matrix.CreateSingleton(derivative1.Evaluate(assignment))
				);
		}
		public override IEnumerable<Matrix> GetHessians(Matrix position)
		{
			Assignment assignment = new MatrixAssignment(variables, position);

			return
				from term in terms
				let derivative0 = term
				select Matrix.FromRowVectors
				(
					from variable1 in variables
					let derivative1 = derivative0.GetDerivative(variable1)
					select Matrix.FromColumnVectors
					(
						from variable2 in variables
						let derivative2 = derivative1.GetDerivative(variable2)
						select Matrix.CreateSingleton(derivative2.Evaluate(assignment))
					)
				);
		}
	}
}

