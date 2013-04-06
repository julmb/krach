using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using Krach.Calculus.Terms.Basic.Definitions;

namespace Krach.Calculus.Rules.Composite
{
	public class Everywhere : Rule
	{
		readonly Rule rule;

		public Rule Rule { get { return rule; } }

        public Everywhere(Rule rule)
		{
			if (rule == null) throw new ArgumentException("rule");

			this.rule = rule;
		}

		public override string ToString()
		{
			return string.Format("[{0}]", rule);
		}
		public override T Rewrite<T>(T term)
		{
			T rewrittenTerm = rule.Rewrite(term);
			if (rewrittenTerm != null) return rewrittenTerm;

			if (term is BasicValueTerm) return null;
			if (term is BasicFunctionTerm) return null;
			if (term is Variable) return null;
			if (term is Abstraction)
			{
				Abstraction abstraction = (Abstraction)(BaseTerm)term;

				ValueTerm rewrittenAbstractionTerm = Rewrite(abstraction.Term);
				if (rewrittenAbstractionTerm != null) return (T)(BaseTerm)new Abstraction(abstraction.Variables, rewrittenAbstractionTerm);
				
				return null;
			}
			if (term is Application)
			{
				Application application = (Application)(BaseTerm)term;

                FunctionTerm rewrittenApplicationFunction = Rewrite(application.Function);
                ValueTerm rewrittenApplicationParameter = Rewrite(application.Parameter);
                if (rewrittenApplicationFunction != null && rewrittenApplicationParameter != null) return (T)(BaseTerm)new Application(rewrittenApplicationFunction, rewrittenApplicationParameter);
				if (rewrittenApplicationFunction != null) return (T)(BaseTerm)new Application(rewrittenApplicationFunction, application.Parameter);
				if (rewrittenApplicationParameter != null) return (T)(BaseTerm)new Application(application.Function, rewrittenApplicationParameter);

				return null;
			}
			if (term is Vector)
			{
				Vector vector = (Vector)(BaseTerm)term;

				IEnumerable<ValueTerm> rewrittenVectorTerms = RewriteAll(vector.Terms);
				if (rewrittenVectorTerms != null) return (T)(BaseTerm)new Vector(rewrittenVectorTerms);

				return null;
			}
			if (term is Selection)
			{
				Selection selection = (Selection)(BaseTerm)term;

				ValueTerm rewrittenSelectionTerm = Rewrite(selection.Term);
				if (rewrittenSelectionTerm != null) return (T)(BaseTerm)new Selection(rewrittenSelectionTerm, selection.Index);

				return null;
			}
			
			throw new InvalidOperationException();
		}

		IEnumerable<T> RewriteAll<T>(IEnumerable<T> terms) where T : VariableTerm<T>
		{
			List<T> result = new List<T>();

			bool matched = false;
			
			foreach (T term in terms) 
			{
				T rewrittenTerm = Rewrite(term);

				if (rewrittenTerm != null)
				{
					matched = true;
						
					result.Add(rewrittenTerm);
				}
				else result.Add(term);
			}

			if (!matched) return null;

			return result;
		}
	}
}

