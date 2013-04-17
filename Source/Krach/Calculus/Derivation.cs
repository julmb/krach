using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms.Notation;
using Krach.Calculus.Abstract;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic.Atoms;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus.Terms.Basic.Definitions;
using Krach.Calculus.Terms.Notation.Custom;
using Krach.Calculus.Terms;

namespace Krach.Calculus
{
	public static class Derivation
	{
		delegate IEnumerable<ValueTerm> ValueTermDerivationRule(ValueTerm term, Variable variable);

		static IEnumerable<ValueTermDerivationRule> ValueTermDerivationRules
		{
			get
			{
				yield return GetDerivativesSum;
				yield return GetDerivativesProduct;
				yield return GetDerivativesExponentiation;
			}
		}

		public static IEnumerable<ValueTerm> GetDerivatives(this ValueTerm term, Variable variable)
		{
			IEnumerable<ValueTerm> result = DeriveAny(ValueTermDerivationRules, term, variable);

			if (result != null) return result;

			if (term is BasicValueTerm)
			{
				BasicValueTerm basicValueTerm = (BasicValueTerm)term;

				return
				(
					from variableIndex in Enumerable.Range(0, variable.Dimension)
					select Term.Constant(Enumerable.Repeat(0.0, basicValueTerm.Dimension))
				)
				.ToArray();
			}
			if (term is Variable)
			{
				Variable variableTerm = (Variable)term;

				return
				(
					from variableIndex in Enumerable.Range(0, variable.Dimension)
					select Term.Vector
					(
						from componentIndex in Enumerable.Range(0, variableTerm.Dimension)
						select Term.Constant(variable == variableTerm && variableIndex == componentIndex ? 1 : 0)
					)
				)
				.ToArray();
			}
			if (term is Application)
			{
				Application applicationTerm = (Application)term;

				IEnumerable<ValueTerm> functionDerivatives =
				(
					from derivative in GetDerivatives(applicationTerm.Function)
					select derivative.Apply(applicationTerm.Parameter)
				)
				.ToArray();
				IEnumerable<ValueTerm> flippedFunctionDerivatives =
				(
					from index in Enumerable.Range(0, applicationTerm.Function.CodomainDimension)
					select Term.Vector
					(
						from derivative in functionDerivatives
						select derivative.Select(index)
					)
				)
				.ToArray();
				IEnumerable<ValueTerm> parameterDerivatives = GetDerivatives(applicationTerm.Parameter, variable);

				return
				(
					from parameterDerivative in parameterDerivatives
					select Term.Vector
					(
						from functionDerivative in flippedFunctionDerivatives
						select Term.DotProduct(functionDerivative, parameterDerivative)
					)
				)
				.ToArray();
			}
			if (term is Vector)
			{
				Vector vectorTerm = (Vector)term;
				
				return
				(
					from subTerm in vectorTerm.Terms
					select GetDerivatives(subTerm, variable)
				)
				.Flip()
				.Select(Term.Vector)
				.ToArray();
			}
			if (term is Selection)
			{
				Selection selectionTerm = (Selection)term;

				return
				(
					from derivative in GetDerivatives(selectionTerm.Term, variable)
					select derivative.Select(selectionTerm.Index)
				)
				.ToArray();
			}

			throw new InvalidOperationException();
		}
		public static IEnumerable<FunctionTerm> GetDerivatives(this FunctionTerm term)
		{
			if (term is Sum)
			{
				Variable x = new Variable(1, "x");
				Variable y = new Variable(1, "y");

				return Enumerables.Create
				(
					Term.Constant(1).Abstract(x, y),
					Term.Constant(1).Abstract(x, y)
				);
			}
			if (term is Product)
			{
				Variable x = new Variable(1, "x");
				Variable y = new Variable(1, "y");

				return Enumerables.Create
				(
					y.Abstract(x, y),
					x.Abstract(x, y)
				);
			}
			if (term is Exponentiation)
			{
				Variable x = new Variable(1, "x");
				Variable y = new Variable(1, "y");
				
				return Enumerables.Create
				(
					Term.Product(y, Term.Exponentiation(x, Term.Difference(y, Term.Constant(1)))).Abstract(x, y),
					Term.Product(Term.Logarithm(x), Term.Exponentiation(x, y)).Abstract(x, y)
				);
			}
			if (term is Logarithm)
			{
				Variable x = new Variable(1, "x");

				return Enumerables.Create(Term.Invert(x).Abstract(x));
			}
			if (term is FunctionDefinition)
			{
				FunctionDefinition functionDefinitionTerm = (FunctionDefinition)term;

				IEnumerable<FunctionTerm> derivatives = GetDerivatives(functionDefinitionTerm.Function).ToArray();

	            return
				(
					from index in Enumerable.Range(0, derivatives.Count())
					let derivative = derivatives.ElementAt(index)
					select new FunctionDefinition
					(
						string.Format("{0}_d{1}", functionDefinitionTerm.Name, index),
						Rewriting.CompleteNormalization.Rewrite(derivative),
						new BasicSyntax(string.Format("{0}'{1}", functionDefinitionTerm.Syntax.GetText(), index.ToString().ToSuperscript()))
					)
				)
				.ToArray();
			}
			if (term is Abstraction)
			{
				Abstraction abstractionTerm = (Abstraction)term;

				return
				(
					from variable in abstractionTerm.Variables
					from derivative in GetDerivatives(abstractionTerm.Term, variable)
					select derivative.Abstract(abstractionTerm.Variables)
				)
				.ToArray();
			}

			throw new InvalidOperationException();
		}

		static IEnumerable<ValueTerm> DeriveAny(IEnumerable<ValueTermDerivationRule> rules, ValueTerm term, Variable variable)
		{
			foreach (ValueTermDerivationRule rule in rules)
			{
				IEnumerable<ValueTerm> derivatives = rule(term, variable);

                if (derivatives != null) return derivatives;
			}

			return null;
		}
		static IEnumerable<ValueTerm> GetDerivativesSum(ValueTerm term, Variable variable)
		{
			if (!(term is Application)) return null;

			Application application = (Application)term;

			if (!(application.Function is Sum)) return null;

			if (!(application.Parameter is Vector)) return null;

			Vector vector = (Vector)application.Parameter;

			if (vector.Terms.Count() != 2) return null;

			ValueTerm term0 = vector.Terms.ElementAt(0);
			ValueTerm term1 = vector.Terms.ElementAt(1);

			return Enumerable.Zip
			(
				term0.GetDerivatives(variable),
				term1.GetDerivatives(variable),
				(derivative0, derivative1) => Term.Sum(derivative0, derivative1)
			);
		}
		static IEnumerable<ValueTerm> GetDerivativesProduct(ValueTerm term, Variable variable)
		{
			if (!(term is Application)) return null;

			Application application = (Application)term;

			if (!(application.Function is Product)) return null;

			if (!(application.Parameter is Vector)) return null;

			Vector vector = (Vector)application.Parameter;

			if (vector.Terms.Count() != 2) return null;

			ValueTerm term0 = vector.Terms.ElementAt(0);
			ValueTerm term1 = vector.Terms.ElementAt(1);

			return Enumerable.Zip
			(
				term0.GetDerivatives(variable),
				term1.GetDerivatives(variable),
				(derivative0, derivative1) => Term.Sum(Term.Product(derivative0, term1), Term.Product(term0, derivative1))
			);
		}
		static IEnumerable<ValueTerm> GetDerivativesExponentiation(ValueTerm term, Variable variable)
		{
			if (!(term is Application)) return null;

			Application application = (Application)term;

			if (!(application.Function is Exponentiation)) return null;

			if (!(application.Parameter is Vector)) return null;

			Vector vector = (Vector)application.Parameter;

			if (vector.Terms.Count() != 2) return null;

			ValueTerm term0 = vector.Terms.ElementAt(0);
			ValueTerm term1 = vector.Terms.ElementAt(1);

			return Enumerable.Zip
			(
				term0.GetDerivatives(variable),
				term1.GetDerivatives(variable),
				(derivative0, derivative1) => Term.Sum
				(
					Term.Product(term1, Term.Exponentiation(term0, Term.Difference(term1, Term.Constant(1))), derivative0),
					Term.Product(Term.Logarithm(term0), Term.Exponentiation(term0, term1), derivative1)
				)
			);
		}
	}
}


