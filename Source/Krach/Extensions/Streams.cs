﻿// Copyright © Julian Brunner 2010 - 2011

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
using System.Text;
using System.Linq;

namespace Krach.Extensions
{
	public static class Streams
	{
		public static byte[] Peek(this BinaryReader reader, int count)
		{
			byte[] result = reader.ReadBytes(count);

			reader.BaseStream.Position -= result.Length;

			return result;
		}
		public static byte PeekByte(this BinaryReader reader)
		{
			byte result = reader.ReadByte();

			reader.BaseStream.Position--;

			return result;
		}
		public static byte[] ReadToZero(this BinaryReader reader, Encoding encoding)
		{
			List<byte> data = new List<byte>();

			while (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				data.Add(reader.ReadByte());
				
				if (encoding.GetString(data.ToArray()).Last() == '\0') return data.SkipLast(encoding.GetByteCount("\0")).ToArray();
			}

			throw new InvalidDataException("Hit end of stream while reading null-terminated string.");
		}
	}
}
