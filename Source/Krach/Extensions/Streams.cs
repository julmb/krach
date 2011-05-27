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

using System.Collections.Generic;
using System.IO;

namespace Krach.Extensions
{
	public static class Streams
	{
		public static byte[] Peek(this BinaryReader reader, int count)
		{
			byte[] result = reader.ReadBytes(count);

			reader.BaseStream.Position -= count;

			return result;
		}
		public static byte[] ReadToPosition(this BinaryReader reader, long position)
		{
			List<byte> result = new List<byte>();

			while (reader.BaseStream.Position < position) result.Add(reader.ReadByte());

			return result.ToArray();
		}
		public static byte[] ReadToNextZero(this BinaryReader reader)
		{
			List<byte> result = new List<byte>();

			while (true)
			{
				byte data = reader.ReadByte();

				if (data == 0) return result.ToArray();

				result.Add(data);
			}
		}
	}
}
