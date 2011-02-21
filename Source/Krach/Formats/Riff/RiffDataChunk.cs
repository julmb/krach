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
	public class RiffDataChunk : RiffChunk
	{
		readonly byte[] data;

		public byte[] Data { get { return data; } }

		public RiffDataChunk(byte[] data) : base("data", (uint)data.Length)
		{
			if (data == null) throw new ArgumentNullException("data");

			this.data = data;
		}
		public RiffDataChunk(BinaryReader reader)
			: base(reader)
		{
			if (ID != "data") throw new ArgumentException(string.Format("Wrong chunk ID '{0}', should be 'data'.", ID));

			this.data = reader.ReadBytes((int)Size);
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			writer.Write(data);
		}
	}
}
