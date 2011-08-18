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

using System.IO;
using System.Collections.Generic;

namespace Krach.Formats.Tags.ID3v2
{
	public class ID3v2GenericFrame : ID3v2Frame
	{
		readonly byte[] data;

		public IEnumerable<byte> Data { get { return data; } }
		public override int DataLength { get { return data.Length; } }

		public ID3v2GenericFrame(BinaryReader reader)
			: base(reader)
		{
			this.data = reader.ReadBytes(ParsedDataLength);
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			writer.Write(data);
		}
	}
}
