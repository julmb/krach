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

using System;
using System.IO;
using System.Text;

namespace Krach.Formats.Tags.Id3v2
{
	public class Id3v2TextFrame : Id3v2Frame
	{
		readonly Encoding encoding;
		readonly string text;

		public Encoding Encoding { get { return encoding; } }
		public string Text { get { return text; } }

		public Id3v2TextFrame(BinaryReader reader)
			: base(reader)
		{
			byte encodingIdentifier = reader.ReadByte();

			switch (encodingIdentifier)
			{
				case 0: this.encoding = Encoding.ASCII; break;
				case 1: this.encoding = Encoding.Unicode; break;
				default: throw new ArgumentException(string.Format("Unknown encoding identifier '{0}'.", encodingIdentifier));
			}

			this.text = encoding.GetString(reader.ReadBytes(DataLength - 1));
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", Identifier, text);
		}
	}
}
