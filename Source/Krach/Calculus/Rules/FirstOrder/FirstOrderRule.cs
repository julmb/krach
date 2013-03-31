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

		public override string ToString()
		{
			return string.Format("({0} -> {1})", originalTerm, rewrittenTerm);
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is ValueTerm)) return null;
			
			ValueTerm valueTerm = (ValueTerm)(BaseTerm)term;
			
			IEnumerable<Assignment> equations = GetEquations(originalTerm, valueTerm);

			if (equations == null) return null;
			
			if (!Substitution.IsAssignment(equations)) return null;

			return (T)(BaseTerm)Substitution.FromEquations(equations).Apply(rewrittenTerm);
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

                List<Assignment> result = new List<Assignment>();

                foreach (IEnumerable<Assignment> equations in Enumerable.Zip(patternVector.Terms, termVector.Terms, GetEquations))
                {
                    if (equations == null) return null;

                    result.AddRange(equations);
                }

                return result;
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

