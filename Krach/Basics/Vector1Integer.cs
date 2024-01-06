// Copyright © Julian Brunner 2010 - 2011

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Xml.Linq;

namespace Krach.Basics
{
	public struct Vector1Integer : IEquatable<Vector1Integer>
	{
		readonly int x;

		public static Vector1Integer Origin { get { return new Vector1Integer(0); } }
		public static Vector1Integer UnitX { get { return new Vector1Integer(1); } }
		public static string XElementName { get { return "vector_1_integer"; } }

		public int X { get { return x; } }
		public XElement XElement { get { return new XElement(XElementName, new XElement("x", x)); } }

		public Vector1Integer(int x)
		{
			this.x = x;
		}
		public Vector1Integer(XElement source)
		{
			if (source == null) throw new ArgumentNullException("source");

			this.x = (int)source.Element("x");
		}

		public override bool Equals(object obj)
		{
			return obj is Vector1Integer && this == (Vector1Integer)obj;
		}
		public override int GetHashCode()
		{
			return x.GetHashCode();
		}
		public override string ToString()
		{
			return "(" + x + ")";
		}
		public bool Equals(Vector1Integer other)
		{
			return this == other;
		}

		public static bool operator ==(Vector1Integer vector1, Vector1Integer vector2)
		{
			return vector1.x == vector2.x;
		}
		public static bool operator !=(Vector1Integer vector1, Vector1Integer vector2)
		{
			return vector1.x != vector2.x;
		}

		public static implicit operator Vector1Double(Vector1Integer vector)
		{
			return new Vector1Double(vector.x);
		}

		public static Vector1Integer operator -(Vector1Integer vector)
		{
			return new Vector1Integer(-vector.x);
		}
		public static Vector1Integer operator +(Vector1Integer vector)
		{
			return new Vector1Integer(+vector.x);
		}
		public static Vector1Integer operator +(Vector1Integer vector1, Vector1Integer vector2)
		{
			return new Vector1Integer(vector1.x + vector2.x);
		}
		public static Vector1Integer operator -(Vector1Integer vector1, Vector1Integer vector2)
		{
			return new Vector1Integer(vector1.x - vector2.x);
		}
		public static Vector1Integer operator *(Vector1Integer vector, double factor)
		{
			return new Vector1Integer((int)(vector.x * factor));
		}
		public static Vector1Integer operator *(double factor, Vector1Integer vector)
		{
			return new Vector1Integer((int)(factor * vector.x));
		}
	}
}
