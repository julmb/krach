using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Calculus.Terms;

namespace Krach.Calculus.Rules.FirstOrder
{
	public class Substitution
	{
		readonly IEnumerable<Assignment> assignments;
		
		Substitution(IEnumerable<Assignment> assignments)
		{
			if (assignments == null) throw new ArgumentNullException("assignments");
			
			this.assignments = assignments.ToArray();
		}
		
		public T Apply<T>(T term) where T : VariableTerm<T>
		{
			return term.Substitute
			(
				assignments.Select(assignment => assignment.Variable),
				assignments.Select(assignment => assignment.Term)
			);
		}
		
		public static bool IsAssignment(IEnumerable<Assignment> equations) 
		{
			return 
			( 
				from equation in equations
				group equation.Term by equation.Variable into variableGroup
				select variableGroup.Distinct().Count() 
			)
			.All(termCount => termCount == 1);
		}
		public static Substitution FromEquations(IEnumerable<Assignment> equations) 
		{			
			return new Substitution
			(
				from equation in equations
				group equation.Term by equation.Variable into variableGroup
				select new Assignment(variableGroup.Key, variableGroup.Distinct().Single())
			);
		}
	}
}

