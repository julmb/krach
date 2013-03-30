using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;

namespace Krach.Calculus.Terms.Notation.Composite
{
	public class SelectionSyntax : ValueSyntax
	{
		readonly ValueTerm term;
		readonly int index;
		
		public SelectionSyntax(ValueTerm term, int index)
		{
			if (term == null) throw new ArgumentNullException("term");
			if (index < 0 || index >= term.Dimension) throw new ArgumentOutOfRangeException("index");
			
			this.term = term;
			this.index = index;
		}

		public override string GetText()
		{
			return string.Format("{0}{1}", term, index.ToString().ToSubscript());
		}
	}
}

