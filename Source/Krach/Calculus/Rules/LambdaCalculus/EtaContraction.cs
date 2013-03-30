using System;
using System.Linq;
using Krach.Calculus.Terms;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Composite;

namespace Krach.Calculus.Rules.LambdaCalculus
{
	public class EtaContraction : Rule
	{
		public override string ToString()
		{
			return "η";
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is Abstraction)) return null;
			
			Abstraction abstraction = (Abstraction)(BaseTerm)term;
			
			if (!(abstraction.Term is Application)) return null;

			Application application = (Application)abstraction.Term;

			IEnumerable<ValueTerm> parameters = application.Parameter is Vector ? ((Vector)application.Parameter).Terms : Enumerables.Create(application.Parameter);

			if (!Enumerable.SequenceEqual(abstraction.Variables, parameters)) return null;

			return (T)(BaseTerm)application.Function;
		}
	}
}

