using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms.Notation.Default;

namespace Krach.Calculus.Terms.Notation
{
    public abstract class Syntax
    {
        public abstract string GetText();
        public virtual Syntax GetApplicationSyntax(Application application)
        {
            return null;
        }

        public static Syntax Variable(Variable variable)
        {
            return new VariableSyntax(variable.Name);
        }
        public static Syntax Abstraction(Abstraction abstraction)
        {
            return new AbstractionSyntax(abstraction.Variables, abstraction.Term);
        }
        public static Syntax Application(Application application)
        {
            Syntax applicationSyntax = application.Function.Syntax.GetApplicationSyntax(application);

            if (applicationSyntax != null) return applicationSyntax;

            return new ApplicationSyntax(application.Function, application.Parameter);
        }
        public static Syntax Vector(Vector vector)
        {
            return new VectorSyntax(vector.Terms);
        }
        public static Syntax Selection(Selection selection)
        {
            return new SelectionSyntax(selection.Term, selection.Index);
        }
    }
}

