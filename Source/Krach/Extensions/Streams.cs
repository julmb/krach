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
using System.Linq;
using System.Text;

namespace Krach.Extensions
{
	public static class Streams
	{
		public static byte[] Peek(this BinaryReader binaryReader, int count)
		{
			byte[] result = binaryReader.ReadBytes(count);

			binaryReader.BaseStream.Position -= result.Length;

			return result;
		}
		public static byte PeekByte(this BinaryReader binaryReader)
		{
			byte result = binaryReader.ReadByte();

			binaryReader.BaseStream.Position--;

			return result;
		}
		public static byte[] ReadToZero(this BinaryReader binaryReader, Encoding encoding)
		{
			List<byte> data = new List<byte>();

			while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
			{
				data.Add(binaryReader.ReadByte());
				
				if (encoding.GetString(data.ToArray()).Last() == '\0') return data.SkipLast(encoding.GetByteCount("\0")).ToArray();
			}

			throw new InvalidDataException("Hit end of stream while reading null-terminated string.");
		}
		public static IEnumerable<string> ReadAllLines(this TextReader textReader)
		{
			while (true)
			{
				string line = textReader.ReadLine();

				if (line == null) break;
				
				yield return line;
			}
		}
	}
}
