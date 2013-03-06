using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Terms.LambdaTerms;
using Krach.Extensions;

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
			if (pattern is Constant && pattern == term) return Enumerable.Empty<Assignment>();			
			if (pattern is Variable) return Enumerables.Create(new Assignment((Variable)pattern, term));
			if (pattern is Application && term is Application) 
			{
				Application patternApplication = (Application)pattern;
				Application termApplication = (Application)term;
				
				if (patternApplication.Function == termApplication.Function)
				{
					return 
					(
						from item in Enumerable.Zip(patternApplication.Parameters, termApplication.Parameters, Tuple.Create)
						from equation in AggregateEquations(item.Item1, item.Item2)
						select equation
					)
					.ToArray();
				}				
			}
			
			throw new InvalidOperationException();
		}
	}
}

