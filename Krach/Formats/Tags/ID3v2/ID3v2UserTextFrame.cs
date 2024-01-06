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

using System.IO;

namespace Krach.Formats.Tags.ID3v2
{
	public class ID3v2UserTextFrame : ID3v2EncodedFrame
	{
		readonly string description;
		readonly string text;

		public string Description { get { return description; } }
		public string Text { get { return text; } }
		public override int DataLength { get { return base.DataLength + TextToData(Encoding, description + '\0').Length + TextToData(Encoding, text).Length; } }

		public ID3v2UserTextFrame(BinaryReader reader)
			: base(reader)
		{
			long headerStartPosition = reader.BaseStream.Position;

			this.description = ReadText(reader, Encoding);

			long headerEndPosition = reader.BaseStream.Position;

			long textDataLength = ParsedDataLength - base.DataLength - (headerEndPosition - headerStartPosition);

			if (textDataLength < 0) throw new InvalidDataException(string.Format("Invalid text data length '{0}'.", textDataLength));

			this.text = ReadText(reader, Encoding, (int)textDataLength);
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			WriteText(writer, Encoding, description + '\0');
			WriteText(writer, Encoding, text);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", description, text);
		}
	}
}
