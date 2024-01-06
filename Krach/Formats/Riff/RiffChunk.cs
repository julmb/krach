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
using System.IO;
using System.Text;

namespace Krach.Formats.Riff
{
	public abstract class RiffChunk
	{
		readonly string id;
		readonly uint size;

		public string ID { get { return id; } }
		public uint Size { get { return size; } }

		protected RiffChunk(string id, uint size)
		{
			if (id.Length != 4) throw new InvalidDataException(string.Format("Parameter id '{0}' doesn't have a length of 4.", id));

			this.id = id;
			this.size = size;
		}
		protected RiffChunk(BinaryReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			this.id = Encoding.ASCII.GetString(reader.ReadBytes(4));;
			this.size = reader.ReadUInt32();
		}

		public virtual void Write(BinaryWriter writer)
		{
			writer.Write(id.ToCharArray());
			writer.Write(size);
		}
	}
}
