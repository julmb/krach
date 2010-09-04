// Copyright © Julian Brunner 2010

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
using System.IO;

namespace Krach.Formats.Riff
{
	public abstract class RiffChunk
	{
		readonly uint size;
		readonly string format;

		public uint Size { get { return size; } }
		public string Format { get { return format; } }

		public RiffChunk(BinaryReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			// RIFF chunk ID
			if (new string(reader.ReadChars(4)) != "RIFF") throw new ArgumentException();
			// RIFF chunk size
			this.size = reader.ReadUInt32();
			// RIFF format ID
			this.format = new string(reader.ReadChars(4));
		}
	}
}
