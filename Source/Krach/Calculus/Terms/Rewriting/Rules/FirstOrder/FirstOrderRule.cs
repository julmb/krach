using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms.Basic;

namespace Krach.Calculus.Terms.Rewriting.Rules.FirstOrder
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
			try { Match(originalTerm, term); }
			catch (InvalidOperationException) { return false; }
			
			return true;
		}
		public override T Rewrite<T>(T term)
		{			
			return (T)(BaseTerm)Match(originalTerm, term).Apply(rewrittenTerm);
		}
	
		static Substitution Match(ValueTerm pattern, BaseTerm term)
		{
			if (!(term is ValueTerm)) throw new InvalidOperationException();
			
			ValueTerm valueTerm = (ValueTerm)term;
			
			if (pattern.Dimension != valueTerm.Dimension) throw new InvalidOperationException();
			
			IEnumerable<Assignment> equations = AggregateEquations(pattern, valueTerm);
			
			if (!Substitution.IsAssignment(equations)) throw new InvalidOperationException();
			
			return Substitution.FromEquations(equations);
		}
		static IEnumerable<Assignment> AggregateEquations(ValueTerm pattern, ValueTerm term)
		{
			if (pattern is BasicValueTerm && term is BasicValueTerm)
			{
				BasicValueTerm patternBasicValueTerm = (BasicValueTerm)pattern;
				BasicValueTerm termBasicValueTerm = (BasicValueTerm)term;
				
				if (patternBasicValueTerm != termBasicValueTerm) throw new InvalidOperationException();
				
				return Enumerables.Create<Assignment>();
			}
			if (pattern is Variable)
			{
				Variable patternVariable = (Variable)pattern;
				
				return Enumerables.Create(new Assignment(patternVariable, term));
			}
			if (pattern is Application && term is Application) 
			{
				Application patternApplication = (Application)pattern;
				Application termApplication = (Application)term;
				
				if (patternApplication.Function != termApplication.Function) throw new InvalidOperationException();
				
				return AggregateEquations(patternApplication.Parameter, termApplication.Parameter);
			}
			if (pattern is Vector && term is Vector)
			{
				Vector patternVector = (Vector)pattern;
				Vector termVector = (Vector)term;
				
				if (patternVector.Terms.Count() != termVector.Terms.Count()) throw new InvalidOperationException();
				
				return
				(
					from equations in Enumerable.Zip(patternVector.Terms, termVector.Terms, AggregateEquations)
					from equation in equations
					select equation
				)
				.ToArray();
			}
			if (pattern is Selection && term is Selection)
			{
				Selection patternSelection = (Selection)pattern;
				Selection termSelection = (Selection)term;
				
				if (patternSelection.Index != termSelection.Index) throw new InvalidOperationException();
				
				return AggregateEquations(patternSelection.Term, termSelection.Term);
			}
			
			throw new InvalidOperationException();
		}
	}
}

