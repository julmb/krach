using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;

namespace Krach.Calculus.Rules.Vectors
{
	public class SelectSingle : Rule
	{
		public override bool Matches<T>(T term)
		{
			if (!(term is Selection)) return false;

			Selection selection = (Selection)(BaseTerm)term;
			
			if (selection.Term.Dimension != 1 || selection.Index != 0) return false;
			
			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is Selection)) throw new InvalidOperationException();

			Selection selection = (Selection)(BaseTerm)term;
			
			if (selection.Term.Dimension != 1 || selection.Index != 0) throw new InvalidOperationException();
			
			return (T)(BaseTerm)selection.Term;
		}
	}
}

