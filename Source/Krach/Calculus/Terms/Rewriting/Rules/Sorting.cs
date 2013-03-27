using System;
using System.Linq;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using System.Collections.Generic;

namespace Krach.Calculus.Terms.Rewriting.Rules
{
	public class Sorting
	{
		public class ProductSimple : Rule
		{
			public override bool Matches<T>(T term)
			{
				try { Rewrite(term); }
				catch (InvalidOperationException) { return false; }
				
				return true;
			}
			public override T Rewrite<T>(T term)
			{
				if (!(term is Application)) throw new InvalidOperationException();
				
				Application application = (Application)(BaseTerm)term;
				
				if (!(application.Function is Product)) throw new InvalidOperationException();
				
				if (!(application.Parameter is Vector)) throw new InvalidOperationException();
				
				Vector vector = (Vector)application.Parameter;
				
				if (vector.Terms.Count() != 2) throw new InvalidOperationException();
				
				ValueTerm parameter0 = vector.Terms.ElementAt(0);
				ValueTerm parameter1 = vector.Terms.ElementAt(1);
				
				if (!ShouldSwap(parameter0, parameter1)) throw new InvalidOperationException();
				
				return (T)(BaseTerm)Term.Product(parameter1, parameter0);
			}
		}
		public class ProductAssociative : Rule
		{
			public override bool Matches<T>(T term)
			{
				try { Rewrite(term); }
				catch (InvalidOperationException) { return false; }
				
				return true;
			}
			public override T Rewrite<T>(T term)
			{
				if (!(term is Application)) throw new InvalidOperationException();
				
				Application application0 = (Application)(BaseTerm)term;
				
				if (!(application0.Function is Product)) throw new InvalidOperationException();
				
				if (!(application0.Parameter is Vector)) throw new InvalidOperationException();
				
				Vector vector0 = (Vector)application0.Parameter;
				
				if (vector0.Terms.Count() != 2) throw new InvalidOperationException();
				
				ValueTerm parameter00 = vector0.Terms.ElementAt(0);
				ValueTerm parameter01 = vector0.Terms.ElementAt(1);
				
				if (!(parameter00 is Application)) throw new InvalidOperationException();
				
				Application application00 = (Application)parameter00;
				
				if (!(application00.Function is Product)) throw new InvalidOperationException();
				
				if (!(application00.Parameter is Vector)) throw new InvalidOperationException();
				
				Vector vector00 = (Vector)application00.Parameter;
				
				if (vector00.Terms.Count() != 2) throw new InvalidOperationException();
				
				ValueTerm parameter000 = vector00.Terms.ElementAt(0);
				ValueTerm parameter001 = vector00.Terms.ElementAt(1);
				
				// ((parameter000 . parameter001) . parameter01)
				
				if (!ShouldSwap(parameter001, parameter01)) throw new InvalidOperationException();
				
				return (T)(BaseTerm)Term.Product(Term.Product(parameter000, parameter01), parameter001);
			}
		}
		
		static bool ShouldSwap(ValueTerm term1, ValueTerm term2)
		{
			if (!(term1 is Constant) && term2 is Constant) return true;
			
			return false;
		}
	}
}

