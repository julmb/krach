using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Notation;

namespace Krach.Calculus.Terms
{
	public abstract class BaseTerm : IEquatable<BaseTerm>
	{
		public abstract Syntax Syntax { get; }

		public override string ToString()
		{
			return Syntax.GetText();
		}
		public override bool Equals(object obj)
		{
			throw new InvalidOperationException();
		}
		public override int GetHashCode()
		{
			throw new InvalidOperationException();
		}
		public bool Equals(BaseTerm other)
		{
			return object.Equals(this, other);
		}
		
		public static bool operator ==(BaseTerm term1, BaseTerm term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(BaseTerm term1, BaseTerm term2)
		{
			return !object.Equals(term1, term2);
		}
	}
}
