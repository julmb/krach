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
using System;

namespace Krach.Formats.Tags.ID3v2
{
	public class ID3v2TextFrame : ID3v2EncodedFrame
	{
		readonly string text;

		public string Text { get { return text; } }

		public ID3v2TextFrame(string identifier, string text)
			: base(identifier, 1 + TextToData(GetEncoding(1), text).Length, 1)
		{
			if (text == null) throw new ArgumentNullException("text");

			this.text = text;
		}
		public ID3v2TextFrame(BinaryReader reader)
			: base(reader)
		{
			this.text = ReadText(reader, Encoding, DataLength - 1);
		}

		public override void Write(BinaryWriter writer)
		{
			base.Write(writer);

			WriteText(writer, Encoding, text);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", Identifier, text);
		}
	}
}
