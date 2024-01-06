using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Basics;
using Krach.Extensions;
using System.Runtime.InteropServices;
using Krach.Design;

namespace Wrappers.Casadi
{
	public class FunctionTerm : IDisposable, IEquatable<FunctionTerm>
	{
		readonly IntPtr function;

		bool disposed = false;

		public IntPtr Function { get { return function; } }
		public int DomainDimension { get { return TermsWrapped.FunctionDomainDimension(this); } }
		public int CodomainDimension { get { return TermsWrapped.FunctionCodomainDimension(this); } }

		public FunctionTerm(IntPtr function)
		{
			if (function == IntPtr.Zero) throw new ArgumentOutOfRangeException("function");

			this.function = function;
		}
		~FunctionTerm()
		{
			Dispose();
		}
		
		public override bool Equals(object obj)
		{
			return obj is FunctionTerm && Equals(this, (FunctionTerm)obj);
		}
		public override int GetHashCode()
		{
			return function.GetHashCode();
		}
		public bool Equals(FunctionTerm other)
		{
			return object.Equals(this, other);
		}
		public override string ToString()
		{
			return TermsWrapped.FunctionToString(this);
		}
		public IEnumerable<FunctionTerm> GetDerivatives()
		{
			return TermsWrapped.FunctionDerivatives(this);
		}
		public FunctionTerm Simplify()
		{
			return TermsWrapped.FunctionSimplify(this);
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;

				TermsWrapped.DisposeFunction(this);
				
				GC.SuppressFinalize(this);
			}
		}

		public static bool operator ==(FunctionTerm functionTerm1, FunctionTerm functionTerm2)
		{
			return object.Equals(functionTerm1, functionTerm2);
		}
		public static bool operator !=(FunctionTerm functionTerm1, FunctionTerm functionTerm2)
		{
			return !object.Equals(functionTerm1, functionTerm2);
		}
		
		static bool Equals(FunctionTerm functionTerm1, FunctionTerm functionTerm2)
		{
			if (object.ReferenceEquals(functionTerm1, functionTerm2)) return true;
			if (object.ReferenceEquals(functionTerm1, null) || object.ReferenceEquals(functionTerm2, null)) return false;
			
			return functionTerm1.function == functionTerm2.function;
		}
	}
}

