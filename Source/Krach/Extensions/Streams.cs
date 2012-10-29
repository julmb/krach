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
using System;

namespace Krach.Extensions
{
	public static class Streams
	{
		public static byte[] Peek(this BinaryReader binaryReader, int byteCount)
		{
			if (binaryReader == null) throw new ArgumentNullException("binaryReader");
			if (byteCount < 0) throw new ArgumentOutOfRangeException("byteCount");

			byte[] result = binaryReader.ReadBytes(byteCount);

			binaryReader.BaseStream.Position -= result.Length;

			return result;
		}
		public static byte PeekByte(this BinaryReader binaryReader)
		{
			if (binaryReader == null) throw new ArgumentNullException("binaryReader");

			byte result = binaryReader.ReadByte();

			binaryReader.BaseStream.Position--;

			return result;
		}
		public static byte[] ReadToZero(this BinaryReader binaryReader, Encoding encoding)
		{
			if (binaryReader == null) throw new ArgumentNullException("binaryReader");
			if (encoding == null) throw new ArgumentNullException("encoding");

			List<byte> data = new List<byte>();

			while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
			{
				data.Add(binaryReader.ReadByte());

				string partialString = encoding.GetString(data.ToArray(), 0, data.Count);
				
				if (partialString.Length > 0 && partialString.Last() == '\0') return data.SkipLast(encoding.GetByteCount("\0")).ToArray();
			}

			throw new InvalidDataException("Hit end of stream while reading null-terminated string.");
		}
		public static IEnumerable<string> ReadAllLines(this TextReader textReader)
		{
			if (textReader == null) throw new ArgumentNullException("textReader");

			while (true)
			{
				string line = textReader.ReadLine();

				if (line == null) break;
				
				yield return line;
			}
		}
		public static char PeekCharacter(this TextReader textReader)
		{
			if (textReader == null) throw new ArgumentNullException("textReader");

			return (char)textReader.Peek();
		}
	}
}
