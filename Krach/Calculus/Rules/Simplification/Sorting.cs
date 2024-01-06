using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using Krach.Calculus.Terms.Basic.Atoms;

namespace Krach.Calculus.Rules.Simplification
{
	public class Sorting
	{
		public class ProductSimple : Rule
		{
			public override string ToString()
			{
				return "sorting_productsimple";
			}
			public override T Rewrite<T>(T term)
			{
				if (!(term is Application)) return null;

				Application application = (Application)(BaseTerm)term;

				if (!(application.Function is Product)) return null;

				if (!(application.Parameter is Vector)) return null;

				Vector vector = (Vector)application.Parameter;

				if (vector.Terms.Count() != 2) return null;

				ValueTerm parameter0 = vector.Terms.ElementAt(0);
				ValueTerm parameter1 = vector.Terms.ElementAt(1);

                if (parameter0 is Application && ((Application)parameter0).Function is Product) return null;

				if (!ShouldSwap(parameter0, parameter1)) return null;

				return (T)(BaseTerm)new Application(new Product(), new Vector(Enumerables.Create(parameter1, parameter0)));
			}
		}
		public class ProductAssociative : Rule
		{
			public override string ToString()
			{
				return "sorting_productassociative";
			}
			public override T Rewrite<T>(T term)
			{
				if (!(term is Application)) return null;

				Application application0 = (Application)(BaseTerm)term;

				if (!(application0.Function is Product)) return null;

				if (!(application0.Parameter is Vector)) return null;

				Vector vector0 = (Vector)application0.Parameter;

				if (vector0.Terms.Count() != 2) return null;

				ValueTerm parameter00 = vector0.Terms.ElementAt(0);
				ValueTerm parameter01 = vector0.Terms.ElementAt(1);

				if (!(parameter00 is Application)) return null;

				Application application00 = (Application)parameter00;

				if (!(application00.Function is Product)) return null;

				if (!(application00.Parameter is Vector)) return null;

				Vector vector00 = (Vector)application00.Parameter;

				if (vector00.Terms.Count() != 2) return null;

				ValueTerm parameter000 = vector00.Terms.ElementAt(0);
				ValueTerm parameter001 = vector00.Terms.ElementAt(1);

				// ((parameter000 . parameter001) . parameter01)

				if (!ShouldSwap(parameter001, parameter01)) return null;

                return (T)(BaseTerm)new Application(new Product(), new Vector(Enumerables.Create(new Application(new Product(), new Vector(Enumerables.Create(parameter000, parameter01))), parameter001)));
			}
		}

		static bool ShouldSwap(ValueTerm term1, ValueTerm term2)
		{
			if (!(term1 is Constant) && term2 is Constant) return true;

			return false;
		}
	}
}

