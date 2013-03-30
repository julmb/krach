using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;

namespace Krach.Calculus.Rules.Composite
{
	public class Anywhere : Rule
	{
		readonly Rule rule;

		public Anywhere(Rule rule)
		{
			if (rule == null) throw new ArgumentException("rule");

			this.rule = rule;
		}

		public override bool Matches<T>(T term)
		{
			if (rule.Matches(term)) return true;
			
			if (term is BasicValueTerm) return false;
			if (term is BasicFunctionTerm) return false;
			if (term is Variable) return false;
			if (term is Abstraction)
			{
				Abstraction abstraction = (Abstraction)(BaseTerm)term;
				
				if (Matches(abstraction.Term)) return true;
				
				return false;
			}
			if (term is Application)
			{
				Application application = (Application)(BaseTerm)term;
				
				if (Matches(application.Function)) return true;
				if (Matches(application.Parameter)) return true;
				
				return false;
			}
			if (term is Vector)
			{
				Vector vector = (Vector)(BaseTerm)term;
				
				if (vector.Terms.Any(Matches)) return true;
				
				return false;
			}
			if (term is Selection)
			{
				Selection selection = (Selection)(BaseTerm)term;
				
				if (Matches(selection.Term)) return true;
				
				return false;
			}
			
			throw new InvalidOperationException();
		}
		public override T Rewrite<T>(T term)
		{
			if (rule.Matches(term))
			{
//				Terminal.Write(term.GetText(), ConsoleColor.Blue);
//				Terminal.WriteLine();

				return rule.Rewrite(term);
			}
			
			if (term is BasicValueTerm) throw new InvalidOperationException();
			if (term is BasicFunctionTerm) throw new InvalidOperationException();
			if (term is Variable) throw new InvalidOperationException();
			if (term is Abstraction)
			{
				Abstraction abstraction = (Abstraction)(BaseTerm)term;
				
				if (Matches(abstraction.Term))
					return (T)(BaseTerm)new Abstraction(abstraction.Variables, Rewrite(abstraction.Term));
				
				throw new InvalidOperationException();
			}
			if (term is Application)
			{
				Application application = (Application)(BaseTerm)term;
				
				if (Matches(application.Function))
					return (T)(BaseTerm)new Application(Rewrite(application.Function), application.Parameter);
				if (Matches(application.Parameter))
					return (T)(BaseTerm)new Application(application.Function, Rewrite(application.Parameter));
				
				throw new InvalidOperationException();
			}
			if (term is Vector)
			{
				Vector vector = (Vector)(BaseTerm)term;
				
				if (vector.Terms.Any(Matches))
					return (T)(BaseTerm)new Vector(RewriteOne(vector.Terms));
				
				throw new InvalidOperationException();
			}
			if (term is Selection)
			{
				Selection selection = (Selection)(BaseTerm)term;
				
				if (Matches(selection.Term))
					return (T)(BaseTerm)new Selection(Rewrite(selection.Term), selection.Index);
				
				throw new InvalidOperationException();
			}
			
			throw new InvalidOperationException();
		}

		IEnumerable<T> RewriteOne<T>(IEnumerable<T> terms) where T : VariableTerm<T>
		{
			bool continueRewriting = true;
			
			foreach (T term in terms) 
			{
				if (continueRewriting && Matches(term))
				{
					continueRewriting = false;
					
					yield return Rewrite(term);
				}
				else yield return term;
			}
			
			if (continueRewriting) throw new InvalidOperationException();
		}
	}
}

