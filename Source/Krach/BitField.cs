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
using System.Collections.Generic;
using System.Linq;
using Krach.Basics;
using Krach.Extensions;

namespace Krach
{
	public struct BitField
	{
		readonly bool[] bits;
		readonly Range<int> range;

		public IEnumerable<byte> Bytes
		{
			get
			{
				if (range.Length() % 8 != 0) throw new InvalidOperationException("Cannot get the bytes of a BitField which has a length that is not a multiple of 8.");

				byte[] bytes = new byte[range.Length() / 8];

				for (int index = range.Start; index < range.End; index++)
				{
					int byteIndex = (index - range.Start) / 8;

					bytes[byteIndex] <<= 1;
					bytes[byteIndex] |= (byte)(bits[index] ? 1 : 0);
				}

				return bytes;
			}
		}
		public IEnumerable<bool> Bits { get { return bits.GetRange(range); } }
		public int Value
		{
			get
			{
				int result = 0;

				for (int index = range.Start; index < range.End; index++)
				{
					result <<= 1;
					result |= bits[index] ? 1 : 0;
				}

				return result;
			}
		}
		public bool this[int position]
		{
			get
			{
				if (position < range.Start || position > range.End) throw new ArgumentOutOfRangeException("position");

				return bits[range.Start + position];
			}
		}
		public BitField this[Range<int> subRange]
		{
			get
			{
				subRange = new Range<int>(subRange.Start + range.Start, subRange.End + range.Start);

				if (subRange.Start < range.Start || subRange.End > range.End) throw new ArgumentException("subRange");

				return new BitField(bits, subRange);
			}
		}
		public BitField this[int startPosition, int endPosition] { get { return this[new Range<int>(startPosition, endPosition)]; } }

		BitField(bool[] bits, Range<int> range)
		{
			if (bits == null) throw new ArgumentNullException("bits");
			if (range.Start < 0 || range.End > bits.Length) throw new ArgumentOutOfRangeException("range");

			this.bits = bits;
			this.range = range;
		}
		BitField(bool[] bits)
		{
			if (bits == null) throw new ArgumentNullException("bits");

			this.bits = bits;
			this.range = new Range<int>(0, bits.Length);
		}

		public override string ToString()
		{
			return Bits.Select(bit => bit ? 1 : 0).ToStrings().AggregateString();
		}

		public static BitField FromBytes(byte[] bytes)
		{
			if (bytes == null) throw new ArgumentNullException("bytes");

			bool[] bits = new bool[bytes.Length * 8];

			for (int index = 0; index < bits.Length; index++)
			{
				int byteIndex = index / 8;
				int bitIndex = (8 - 1) - index % 8;

				bits[index] = (bytes[byteIndex] & (1 << bitIndex)) != 0;
			}

			return new BitField(bits);
		}
		public static BitField FromBytes(IEnumerable<byte> bytes)
		{
			if (bytes == null) throw new ArgumentNullException("bytes");

			return FromBytes(bytes.ToArray());
		}
		public static BitField FromBits(bool[] bits)
		{
			if (bits == null) throw new ArgumentNullException("bits");

			return new BitField(bits);
		}
		public static BitField FromBits(IEnumerable<bool> bits)
		{
			if (bits == null) throw new ArgumentNullException("bits");

			return FromBits(bits.ToArray());
		}
	}
}
