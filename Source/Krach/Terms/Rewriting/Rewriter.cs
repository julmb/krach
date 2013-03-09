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
//			Console.WriteLine("Starting rewrite");
//			Console.WriteLine(term.GetText());
			
			while (CanRewrite(term)) 
			{
				term = DoRewrite(term);
			
//				Console.WriteLine(term.GetText());	
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
			
			if (term is Variable) return false;
			if (term is Abstraction) 
			{
				Abstraction abstraction = (Abstraction)(object)term;
				
				if (CanRewrite(abstraction.Term)) return true;
			}
			if (term is Application) 
			{
				Application application = (Application)(object)term;
				
				if (CanRewrite(application.Function)) return true;
				if (CanRewrite(application.Parameter)) return true;
			}
			if (term is Vector)
			{
				Vector vector = (Vector)(object)term;
				
				if (vector.Values.Any(CanRewrite)) return true;
			}
			if (term is Selection)
			{
				Selection selection = (Selection)(object)term;
				
				if (CanRewrite(selection.Value)) return true;
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
			
			if (term is Variable) throw new InvalidOperationException();
			if (term is Abstraction) 
			{
				Abstraction abstraction = (Abstraction)(object)term;
				
				if (CanRewrite(abstraction.Term)) return (T)(object)DoRewrite(abstraction.Term).Abstract(abstraction.Variable);
			}
			if (term is Application) 
			{
				Application application = (Application)(object)term;
				
				if (CanRewrite(application.Function)) return (T)(object)DoRewrite(application.Function).Apply(application.Parameter);
				if (CanRewrite(application.Parameter)) return (T)(object)application.Function.Apply(DoRewrite(application.Parameter));
			}
			if (term is Vector)
			{
				Vector vector = (Vector)(object)term;
				
				if (vector.Values.Any(CanRewrite)) return (T)(object)Term.Vector(DoRewriteOne(vector.Values));
			}
			if (term is Selection)
			{
				Selection selection = (Selection)(object)term;

				if (CanRewrite(selection.Value)) return (T)(object)DoRewrite(selection.Value).Select(selection.Index);
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

