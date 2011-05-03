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
		readonly IEnumerable<bool> bits;

		public IEnumerable<bool> Bits { get { return bits; } }
		public int Value
		{
			get
			{
				int index = 0;
				int result = 0;
				foreach (bool bit in bits.Reverse()) result += (bit ? 1 : 0) << index++;

				return result;
			}
		}

		public BitField(IEnumerable<bool> bits)
		{
			if (bits == null) throw new ArgumentNullException("bits");

			this.bits = bits;
		}

		public BitField GetRange(int startPosition, int endPosition)
		{
			if (startPosition < 0 || startPosition > bits.Count()) throw new ArgumentOutOfRangeException("startPosition");
			if (endPosition < 0 || endPosition > bits.Count()) throw new ArgumentOutOfRangeException("endPosition");
			if (endPosition < startPosition) throw new ArgumentException("Parameter 'endPosition' cannot be smaller than parameter 'startPosition'.");

			return new BitField(bits.GetRange(startPosition, endPosition));
		}
		public bool GetBit(int position)
		{
			return bits.ElementAt(position);
		}
		public override string ToString()
		{
			return bits.Select(bit => bit ? 1 : 0).ToStrings().AggregateString();
		}

		public static BitField FromBytes(IEnumerable<byte> data)
		{
			if (data == null) throw new ArgumentNullException("data");

			return FromBytes(data, 0, data.Count() * 8);
		}
		public static BitField FromBytes(IEnumerable<byte> data, int startPosition, int endPosition)
		{
			if (data == null) throw new ArgumentNullException("data");

			byte[] bytes = data.ToArray();

			if (startPosition < 0 || startPosition > bytes.Length * 8) throw new ArgumentOutOfRangeException("startPosition");
			if (endPosition < 0 || endPosition > bytes.Length * 8) throw new ArgumentOutOfRangeException("endPosition");
			if (endPosition < startPosition) throw new ArgumentException("Parameter 'endPosition' cannot be smaller than parameter 'startPosition'.");

			List<bool> bits = new List<bool>();

			for (int position = startPosition; position < endPosition; position++)
			{
				int byteIndex = position / 8;
				int bitIndex = 7 - position % 8;

				bits.Add((bytes[byteIndex] & (1 << bitIndex)) != 0);
			}

			return new BitField(bits);
		}
	}
}
