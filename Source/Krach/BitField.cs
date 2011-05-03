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
using Krach.Extensions;

namespace Krach
{
	public class BitField
	{
		readonly bool[] bits;

		public IEnumerable<bool> Bits { get { return bits; } }
		public int Value
		{
			get
			{
				int result = 0;

				for (int index = 0; index < bits.Length; index++) result += (bits[index] ? 1 : 0) << ((bits.Length - 1) - index);

				return result;
			}
		}

		public BitField(IEnumerable<bool> bits)
		{
			if (bits == null) throw new ArgumentNullException("bits");

			this.bits = bits.ToArray();
		}
		BitField(bool[] bits)
		{
			this.bits = bits;
		}

		public BitField GetRange(int startPosition, int endPosition)
		{
			if (startPosition < 0 || startPosition > bits.Length) throw new ArgumentOutOfRangeException("startPosition");
			if (endPosition < 0 || endPosition > bits.Length) throw new ArgumentOutOfRangeException("endPosition");
			if (endPosition < startPosition) throw new ArgumentException("Parameter 'endPosition' cannot be smaller than parameter 'startPosition'.");

			bool[] data = new bool[endPosition - startPosition];

			Array.Copy(bits, startPosition, data, 0, data.Length);

			return new BitField(data);
		}
		public bool GetBit(int position)
		{
			return bits[position];
		}
		public override string ToString()
		{
			return bits.Select(bit => bit ? 1 : 0).ToStrings().AggregateString();
		}

		public static BitField FromBytes(IEnumerable<byte> data)
		{
			if (data == null) throw new ArgumentNullException("data");

			byte[] bytes = data.ToArray();
			bool[] bits = new bool[bytes.Length * 8];

			for (int position = 0; position < bits.Length; position++)
			{
				int byteIndex = position / 8;
				int bitIndex = 7 - position % 8;

				bits[position] = (bytes[byteIndex] & (1 << bitIndex)) != 0;
			}

			return new BitField(bits);
		}
	}
}
