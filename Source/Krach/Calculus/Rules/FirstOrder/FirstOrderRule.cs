using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms.Basic;

namespace Krach.Calculus.Rules.FirstOrder
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
			return CanMatch(originalTerm, term);
		}
		public override T Rewrite<T>(T term)
		{			
			return (T)(BaseTerm)Match(originalTerm, term).Apply(rewrittenTerm);
		}
	
		static bool CanMatch(ValueTerm pattern, BaseTerm term)
		{
			if (!(term is ValueTerm)) return false;
			
			ValueTerm valueTerm = (ValueTerm)term;
			
			if (pattern.Dimension != valueTerm.Dimension) return false;
			
			IEnumerable<Assignment> equations = GetEquations(pattern, valueTerm);

			if (equations == null) return false;

			if (!Substitution.IsAssignment(equations)) return false;
			
			return true;
		}
		static Substitution Match(ValueTerm pattern, BaseTerm term)
		{
			if (!(term is ValueTerm)) throw new InvalidOperationException();
			
			ValueTerm valueTerm = (ValueTerm)term;
			
			if (pattern.Dimension != valueTerm.Dimension) throw new InvalidOperationException();
			
			IEnumerable<Assignment> equations = GetEquations(pattern, valueTerm);

			if (equations == null) throw new InvalidOperationException();
			
			if (!Substitution.IsAssignment(equations)) throw new InvalidOperationException();
			
			return Substitution.FromEquations(equations);
		}
		static IEnumerable<Assignment> GetEquations(ValueTerm pattern, ValueTerm term)
		{
			if (pattern is BasicValueTerm)
			{
				if (!(term is BasicValueTerm)) return null;

				BasicValueTerm patternBasicValueTerm = (BasicValueTerm)pattern;
				BasicValueTerm termBasicValueTerm = (BasicValueTerm)term;
				
				if (patternBasicValueTerm != termBasicValueTerm) return null;
				
				return Enumerables.Create<Assignment>();
			}
			if (pattern is Variable)
			{
				Variable patternVariable = (Variable)pattern;
				
				return Enumerables.Create(new Assignment(patternVariable, term));
			}
			if (pattern is Application)
			{
				if (!(term is Application)) return null;

				Application patternApplication = (Application)pattern;
				Application termApplication = (Application)term;
				
				if (patternApplication.Function != termApplication.Function) return null;
				
				return GetEquations(patternApplication.Parameter, termApplication.Parameter);
			}
			if (pattern is Vector)
			{
				if (!(term is Vector)) return null;

				Vector patternVector = (Vector)pattern;
				Vector termVector = (Vector)term;
				
				if (patternVector.Terms.Count() != termVector.Terms.Count()) return null;

				IEnumerable<IEnumerable<Assignment>> equationSets = Enumerable.Zip(patternVector.Terms, termVector.Terms, GetEquations).ToArray();

				if (equationSets.Any(equations => equations == null)) return null;

				return
				(
					from equations in equationSets
					from equation in equations
					select equation
				)
				.ToArray();
			}
			if (pattern is Selection)
			{
				if (!(term is Selection)) return null;

				Selection patternSelection = (Selection)pattern;
				Selection termSelection = (Selection)term;
				
				if (patternSelection.Index != termSelection.Index) return null;
				
				return GetEquations(patternSelection.Term, termSelection.Term);
			}
			
			throw new InvalidOperationException();
		}
	}
}

