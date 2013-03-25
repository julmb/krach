using System;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;

namespace Krach.Calculus.Terms.Rewriting.Rules
{
	public class SelectVector : Rule
	{
		public override bool Matches<T>(T term)
		{
			try { Rewrite(term); }
			catch (InvalidOperationException) { return false; }
			
			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (term is Selection) 
			{
				Selection selection = (Selection)(BaseTerm)term;
				
				if (selection.Term is Vector)
				{
					Vector vector = (Vector)selection.Term;
					
					IEnumerable<ValueTerm> values =
					(
						from subTerm in vector.Terms
						from index in Enumerable.Range(0, subTerm.Dimension)
						select subTerm.Select(index)
					)
					.ToArray();
					
					return (T)(BaseTerm)values.ElementAt(selection.Index);
				}
			}
			
			throw new InvalidOperationException();
		}
	}
	public class SingletonVector : Rule
	{
		public override bool Matches<T>(T term)
		{
			try { Rewrite(term); }
			catch (InvalidOperationException) { return false; }
			
			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (term is Vector) 
			{
				Vector vector = (Vector)(BaseTerm)term;
				
				if (vector.Terms.Count() == 1) return (T)(BaseTerm)vector.Terms.Single();
			}
			
			throw new InvalidOperationException();
		}
	}
	public class SelectSingle : Rule
	{
		public override bool Matches<T>(T term)
		{
			try { Rewrite(term); }
			catch (InvalidOperationException) { return false; }
			
			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (term is Selection) 
			{
				Selection selection = (Selection)(BaseTerm)term;
				
				if (selection.Term.Dimension == 1 && selection.Index == 0)
					return (T)(BaseTerm)selection.Term;
			}
			
			throw new InvalidOperationException();
		}
	}
	public class FlattenVector : Rule
	{
		public override bool Matches<T>(T term)
		{
			try { Rewrite(term); }
			catch (InvalidOperationException) { return false; }
			
			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (term is Vector) 
			{
				Vector vector = (Vector)(BaseTerm)term;
				
				if (vector.Terms.Any(subTerm => subTerm is Vector))
					return (T)(BaseTerm)Term.Vector
					(
						from subTerm in vector.Terms
						let subSubTerms = subTerm is Vector ? ((Vector)subTerm).Terms : Enumerables.Create(subTerm)
						from subSubTerm in subSubTerms
						select subSubTerm
					);
			}
			
			throw new InvalidOperationException();
		}
	}
}

