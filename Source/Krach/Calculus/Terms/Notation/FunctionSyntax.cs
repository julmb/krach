using System;
using System.Collections.Generic;

namespace Krach.Calculus.Terms.Notation
{
	public abstract class FunctionSyntax : Syntax
	{
		public abstract ValueSyntax GetApplicationSyntax(ValueTerm parameter);
	}
}

