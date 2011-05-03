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
using System.Linq;
using System.Text;

namespace Krach.Formats.Tags.Id3v2
{
	public class Id3v2UserTextFrame : Id3v2Frame
	{
		readonly Encoding encoding;
		readonly string description;
		readonly string text;

		public Encoding Encoding { get { return encoding; } }
		public string Description { get { return description; } }
		public string Text { get { return text; } }

		public Id3v2UserTextFrame(BinaryReader reader)
			: base(reader)
		{
			byte encodingIdentifier = reader.ReadByte();

			switch (encodingIdentifier)
			{
				case 0: this.encoding = Encoding.ASCII; break;
				case 1: this.encoding = Encoding.Unicode; break;
				default: throw new ArgumentException(string.Format("Unknown encoding identifier '{0}'.", encodingIdentifier));
			}

			byte[] data = reader.ReadBytes(DataLength - 1);
			byte[] descriptionData = data.TakeWhile(item => item != 0).ToArray();
			byte[] textData = data.SkipWhile(item => item != 0).Skip(1).ToArray();

			this.description = encoding.GetString(descriptionData);
			this.text = encoding.GetString(textData);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", description, text);
		}
	}
}
