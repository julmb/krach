// Copyright © Julian Brunner 2009 - 2011

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
// 
// You should have received a copy of the GNU General Public License along with
// Stream Visualizer. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;

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
			if (id.Length != 4) throw new ArgumentException(string.Format("Parameter id '{0}' doesn't have a length of 4.", id));

			this.id = id;
			this.size = size;
		}
		protected RiffChunk(BinaryReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			this.id = new string(reader.ReadChars(4));
			this.size = reader.ReadUInt32();
		}

		public virtual void Write(BinaryWriter writer)
		{
			writer.Write(id.ToCharArray());
			writer.Write(size);
		}
	}
}
