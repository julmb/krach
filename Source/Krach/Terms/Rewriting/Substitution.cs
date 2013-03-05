using System;
using System.Collections.Generic;
using System.Linq;

namespace Krach.Terms.Rewriting
{
	public class Substitution<T> where T : Term<T>
	{
		readonly IEnumerable<Assignment> assignments;
		
		Substitution(IEnumerable<Assignment> assignments)
		{
			if (assignments == null) throw new ArgumentNullException("assignments");
			
			this.assignments = assignments;
		}
		
		public T Apply(T term) 
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
		public static Substitution<T> FromEquations(IEnumerable<Assignment> equations) 
		{			
			return new Substitution<T>
			(
				( 
					from equation in equations
					group equation.Term by equation.Variable into variableGroup
					select new Assignment(variableGroup.Key, variableGroup.Distinct().Single())
				)
				.ToArray()
			);
		}
	}
}

