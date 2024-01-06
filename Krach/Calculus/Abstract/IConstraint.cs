using System;
using Krach.Basics;
using Krach.Calculus;
using System.Collections.Generic;
using Krach.Calculus.Terms;
using Krach.Extensions;
using System.Linq;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Abstract;

namespace Krach.Calculus.Abstract
{
	public interface IConstraint<out T>
	{
		T Item { get; }
		IEnumerable<OrderedRange<double>> Ranges { get; }
	}
}

