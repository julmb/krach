using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Basics;
using Krach.Extensions;
using System.Runtime.InteropServices;
using Krach.Design;

namespace Wrappers.Casadi
{
	public class ValueTerm : IDisposable, IEquatable<ValueTerm>
	{
		readonly IntPtr value;

		bool disposed = false;

		public IntPtr Value { get { return value; } }
		public int Dimension { get { return TermsWrapped.ValueDimension(this); } }

		public ValueTerm(IntPtr value)
		{
			if (value == IntPtr.Zero) throw new ArgumentOutOfRangeException("value");

			this.value = value;
		}
		~ValueTerm()
		{
			Dispose();
		}

		public override bool Equals(object obj)
		{
			return obj is ValueTerm && Equals(this, (ValueTerm)obj);
		}
		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
		public bool Equals(ValueTerm other)
		{
			return object.Equals(this, other);
		}
		public override string ToString()
		{
			return TermsWrapped.ValueToString(this);
		}
		public IEnumerable<double> Evaluate()
		{
			return TermsWrapped.ValueEvaluate(this);
		}
		public ValueTerm Simplify()
		{
			return TermsWrapped.ValueSimplify(this);
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;

				TermsWrapped.DisposeValue(this);
				
				GC.SuppressFinalize(this);
			}
		}
	
		public static bool operator ==(ValueTerm valueTerm1, ValueTerm valueTerm2)
		{
			return object.Equals(valueTerm1, valueTerm2);
		}
		public static bool operator !=(ValueTerm valueTerm1, ValueTerm valueTerm2)
		{
			return !object.Equals(valueTerm1, valueTerm2);
		}
		
		static bool Equals(ValueTerm valueTerm1, ValueTerm valueTerm2)
		{
			if (object.ReferenceEquals(valueTerm1, valueTerm2)) return true;
			if (object.ReferenceEquals(valueTerm1, null) || object.ReferenceEquals(valueTerm2, null)) return false;
			
			return valueTerm1.value == valueTerm2.value;
		}
	}
}

