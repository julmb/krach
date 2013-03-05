using System;
using System.Collections.Generic;
using Krach.Terms.LambdaTerms;
using System.Linq;
using Krach.Extensions;

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
		
		public T Rewrite<T>(T term) where T : Term<T> 
		{
			while (CanRewrite(term)) 
			{
				term = DoRewrite(term);
			
				Console.WriteLine(term.GetText());	
			}
			
			return term;
		}
		
		bool CanRewrite<T>(T term) where T : Term<T>
		{
			IEnumerable<Rule> matchingRules = 
				from rule in rules
				where rule.Matches(term)
				select rule;
			
			if (matchingRules.Any()) return true;
			
			if (term is Constant) return false;
			if (term is Variable) return false;
			if (term is Abstraction) 
			{
				Abstraction abstraction = (Abstraction)(object)term;
				
				return CanRewrite(abstraction.Term);
			}
			if (term is Application) 
			{
				Application application = (Application)(object)term;
				
				if (CanRewrite(application.Function)) return true;
				
				return application.Parameters.Any(parameter => CanRewrite(parameter));
			}
			
			return false;
		}
		
		T DoRewrite<T>(T term) where T : Term<T> 
		{
			IEnumerable<Rule> matchingRules = 
				from rule in rules
				where rule.Matches(term)
				select rule;
			
			if (matchingRules.Any()) return matchingRules.First().Rewrite(term);
			
			if (term is Constant) throw new InvalidOperationException();
			if (term is Variable) throw new InvalidOperationException();
			if (term is Abstraction) 
			{
				Abstraction abstraction = (Abstraction)(object)term;
				
				return (T)(object)DoRewrite(abstraction.Term).Abstract(abstraction.Variables);
			}
			if (term is Application) 
			{
				Application application = (Application)(object)term;
				
				if (CanRewrite(application.Function)) return (T)(object)DoRewrite(application.Function).Apply(application.Parameters);
				
				return (T)(object)application.Function.Apply(DoRewriteOne(application.Parameters));
			}
			
			throw new InvalidOperationException();
		}
		
		IEnumerable<T> DoRewriteOne<T>(IEnumerable<T> terms) where T : Term<T> 
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

