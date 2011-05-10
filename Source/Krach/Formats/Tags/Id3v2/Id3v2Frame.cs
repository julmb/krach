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
using System.Text.RegularExpressions;
using Krach;
using System.Text;

namespace Krach.Formats.Tags.Id3v2
{
	public abstract class Id3v2Frame
	{
		readonly string identifier;
		readonly int length;
		readonly bool tagAlterPreservation;
		readonly bool fileAlterPreservation;
		readonly bool readOnly;
		readonly bool compression;
		readonly bool encryption;
		readonly bool groupingIdentity;

		public string Identifier { get { return identifier; } }
		public int TotalLength { get { return length + 10; } }
		public int DataLength { get { return length; } }
		public bool TagAlterPreservation { get { return tagAlterPreservation; } }
		public bool FileAlterPreservation { get { return fileAlterPreservation; } }
		public bool ReadOnly { get { return readOnly; } }
		public bool Compression { get { return compression; } }
		public bool Encryption { get { return encryption; } }
		public bool GroupingIdentity { get { return groupingIdentity; } }

		public Id3v2Frame(BinaryReader reader)
		{
			this.identifier = Encoding.ASCII.GetString(reader.ReadBytes(4));
			if (!Regex.IsMatch(identifier, "^[A-Z0-9]{4}$")) throw new ArgumentException(string.Format("Invalid frame identifier '{0}'", identifier));

			this.length = BitField.FromBytes(reader.ReadBytes(4)).Value;

			BitField flags = BitField.FromBytes(reader.ReadBytes(2));
			if (flags.GetRange(3, 8).Value != 0 || flags.GetRange(11, 16).Value != 0) throw new ArgumentException(string.Format("Found non-used but set flags '{0}'", flags));

			this.tagAlterPreservation = !flags.GetBit(0);
			this.fileAlterPreservation = !flags.GetBit(1);
			this.readOnly = flags.GetBit(2);
			this.compression = flags.GetBit(8);
			this.encryption = flags.GetBit(9);
			this.groupingIdentity = flags.GetBit(10);

			// TODO:
			if (compression || encryption || groupingIdentity) throw new NotImplementedException();
		}
	}
}
