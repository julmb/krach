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

namespace Krach.Formats.Tags.Id3v2
{
	public class Id3v2TextFrame : Id3v2Frame
	{
		readonly byte encodingID;
		readonly string text;

		public byte EncodingID { get { return encodingID; } }
		public string Text { get { return text; } }

		public Id3v2TextFrame(BinaryReader reader)
			: base(reader)
		{
			this.encodingID = reader.ReadByte();
			this.text = GetEncoding(encodingID).GetString(reader.ReadBytes(DataLength - 1));
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			writer.Write(encodingID);
			writer.Write(GetEncoding(encodingID).GetBytes(text));
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", Identifier, text);
		}
	}
}
