using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms.Basic;

namespace Krach.Terms.Rewriting
{
	public class Rewriter
	{
		readonly IEnumerable<Rule> rules;
		
		public Rewriter(IEnumerable<Rule> rules)
		{
			if (rules == null) throw new ArgumentNullException("rules");
			
			this.rules = rules;
		}
		
		public T Rewrite<T>(T term) where T : VariableTerm<T> 
		{
			Console.WriteLine(term.GetText());
			
			while (CanRewrite(term)) 
			{
				term = DoRewrite(term);
			
				Console.WriteLine(term.GetText());
			}
			
			return term;
		}
		
		bool CanRewrite<T>(T term) where T : VariableTerm<T>
		{
			IEnumerable<Rule> matchingRules =
			(
				from rule in rules
				where rule.Matches(term)
				select rule
			);
			
			if (matchingRules.Any()) return true;
			
			if (term is BasicValue) return false;
			if (term is BasicFunction) return false;
			if (term is Variable) return false;
			if (term is Abstraction)
			{
				Abstraction abstraction = (Abstraction)(BaseTerm)term;
				
				if (CanRewrite(abstraction.Term)) return true;
				
				return false;
			}
			if (term is Application)
			{
				Application application = (Application)(BaseTerm)term;
				
				if (CanRewrite(application.Function)) return true;
				if (CanRewrite(application.Parameter)) return true;
				
				return false;
			}
			if (term is Vector)
			{
				Vector vector = (Vector)(BaseTerm)term;
				
				if (vector.Terms.Any(CanRewrite)) return true;
				
				return false;
			}
			if (term is Selection)
			{
				Selection selection = (Selection)(BaseTerm)term;
				
				if (CanRewrite(selection.Term)) return true;
				
				return false;
			}
			
			throw new InvalidOperationException();
		}
		T DoRewrite<T>(T term) where T : VariableTerm<T> 
		{			
			IEnumerable<Rule> matchingRules =
			(
				from rule in rules
				where rule.Matches(term)
				select rule
			);
			
			if (matchingRules.Any()) return matchingRules.First().Rewrite(term);
			
			if (term is BasicValue) throw new InvalidOperationException();
			if (term is BasicFunction) throw new InvalidOperationException();
			if (term is Variable) throw new InvalidOperationException();
			if (term is Abstraction)
			{
				Abstraction abstraction = (Abstraction)(BaseTerm)term;
				
				if (CanRewrite(abstraction.Term))
					return (T)(BaseTerm)new Abstraction(abstraction.Variables, DoRewrite(abstraction.Term));
				
				throw new InvalidOperationException();
			}
			if (term is Application)
			{
				Application application = (Application)(BaseTerm)term;
				
				if (CanRewrite(application.Function))
					return (T)(BaseTerm)new Application(DoRewrite(application.Function), application.Parameter);
				if (CanRewrite(application.Parameter))
					return (T)(BaseTerm)new Application(application.Function, DoRewrite(application.Parameter));
				
				throw new InvalidOperationException();
			}
			if (term is Vector)
			{
				Vector vector = (Vector)(BaseTerm)term;
				
				if (vector.Terms.Any(CanRewrite))
					return (T)(BaseTerm)new Vector(DoRewriteOne(vector.Terms));
				
				throw new InvalidOperationException();
			}
			if (term is Selection)
			{
				Selection selection = (Selection)(BaseTerm)term;
				
				if (CanRewrite(selection.Term))
					return (T)(BaseTerm)new Selection(DoRewrite(selection.Term), selection.Index);
				
				throw new InvalidOperationException();
			}
			
			throw new InvalidOperationException();
		}
		IEnumerable<T> DoRewriteOne<T>(IEnumerable<T> terms) where T : VariableTerm<T> 
		{
			bool continueRewriting = true;
			
			foreach (T term in terms) 
			{
				if (continueRewriting && CanRewrite(term))
				{
					continueRewriting = false;
					
					yield return DoRewrite(term);
				}
				else yield return term;
			}
			
			if (continueRewriting) throw new InvalidOperationException();
		}
	}
}

