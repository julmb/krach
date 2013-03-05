using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Terms.LambdaTerms;
using Krach.Extensions;

namespace Krach.Terms.Rewriting
{
	public class FirstOrderRule : Rule
	{
		readonly ValueTerm originalTerm;
		readonly ValueTerm rewrittenTerm;
		
		public FirstOrderRule(ValueTerm originalTerm, ValueTerm rewrittenTerm)
		{
			if (originalTerm == null) throw new ArgumentNullException("originalTerm");
			if (rewrittenTerm == null) throw new ArgumentNullException("rewrittenTerm");
			
			this.originalTerm = originalTerm;
			this.rewrittenTerm = rewrittenTerm;
		}
		
		public override bool Matches<T>(T term)
		{
			if (!(term is ValueTerm)) return false;
			
			try { return Substitution<ValueTerm>.IsAssignment(AggregateEquations(originalTerm, (ValueTerm)(object)term)); }
			catch (InvalidOperationException) { return false; }
		}
		public override T Rewrite<T>(T term)
		{
			return (T)(object)Substitution<ValueTerm>.FromEquations(AggregateEquations(originalTerm, (ValueTerm)(object)term)).Apply(rewrittenTerm);
		}
		
		static IEnumerable<Assignment> AggregateEquations(ValueTerm pattern, ValueTerm term)
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

