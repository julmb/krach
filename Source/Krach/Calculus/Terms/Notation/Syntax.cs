using System;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;
using Krach.Calculus.Terms.Notation.Composite;

namespace Krach.Calculus.Terms.Notation
{
	public abstract class Syntax
	{
		public abstract string GetText();

		// explanation
		// the application is not the only sensible place for specifying custom syntax
		// having the function of an application term specify this syntax is an even more special case
		// thus, all the responsibility for selecting the right syntax based on the various requests
		// made by the subterms is condensed here
		public static ValueSyntax Variable(Variable variable)
		{
			return new VariableSyntax(variable.Name);
		}
		public static FunctionSyntax Abstraction(Abstraction abstraction)
		{
			return new AbstractionSyntax(abstraction.Variables, abstraction.Term);
		}
		public static ValueSyntax Application(Application application)
		{
			ValueSyntax applicationSyntax = application.Function.FunctionSyntax.GetApplicationSyntax(application.Parameter);

			if (applicationSyntax != null) return applicationSyntax;

			return new ApplicationSyntax(application.Function, application.Parameter);
		}
		public static ValueSyntax Vector(Vector vector)
		{
			return new VectorSyntax(vector.Terms);
		}
		public static ValueSyntax Selection(Selection selection)
		{
			return new SelectionSyntax(selection.Term, selection.Index);
		}
	}
}

