using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Terms.LambdaTerms;
using Krach.Extensions;
using Krach.Terms.Functions;

namespace Krach.Terms.Rewriting
{
	public class FirstOrderRule : Rule
	{
		readonly Value originalTerm;
		readonly Value rewrittenTerm;
		
		public FirstOrderRule(Value originalTerm, Value rewrittenTerm)
		{
			if (originalTerm == null) throw new ArgumentNullException("originalTerm");
			if (rewrittenTerm == null) throw new ArgumentNullException("rewrittenTerm");
			
			this.originalTerm = originalTerm;
			this.rewrittenTerm = rewrittenTerm;
		}
		
		public override bool Matches<T>(T term)
		{
			if (!(term is Value)) return false;
			
			try { return Substitution<Value>.IsAssignment(AggregateEquations(originalTerm, (Value)(object)term)); }
			catch (InvalidOperationException) { return false; }
		}
		public override T Rewrite<T>(T term)
		{
			return (T)(object)Substitution<Value>.FromEquations(AggregateEquations(originalTerm, (Value)(object)term)).Apply(rewrittenTerm);
		}
		
		static IEnumerable<Assignment> AggregateEquations(Value pattern, Value term)
		{
			if (pattern.Dimension == term.Dimension) 
			{
				if (pattern is Variable) return Enumerables.Create(new Assignment((Variable)pattern, term));
				if (pattern is Application && term is Application) 
				{
					Application patternApplication = (Application)pattern;
					Application termApplication = (Application)term;
					
					if (patternApplication.Function == termApplication.Function)
						return AggregateEquations(patternApplication.Parameter, termApplication.Parameter);			
				}
				if (pattern is Vector) 
				{
					Vector patternVector = (Vector)pattern;
					
					return 
					(
						from index in Enumerable.Range(0, patternVector.Dimension)
						from equation in AggregateEquations(patternVector.Values.ElementAt(index), term.Select(index))
						select equation
					)
					.ToArray();
				}
			}
			
			throw new InvalidOperationException();
		}
	}
}

