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
using System.Text.RegularExpressions;
using Krach.Extensions;

namespace Krach.Formats.Tags.ID3v2
{
	public abstract class ID3v2Frame
	{
		readonly string identifier;
		readonly int dataLength;
		readonly bool tagAlterPreservation;
		readonly bool fileAlterPreservation;
		readonly bool readOnly;
		readonly bool compression;
		readonly bool encryption;
		readonly bool groupingIdentity;

		public int HeaderLength { get { return 10; } }
		public string Identifier { get { return identifier; } }
		public int DataLength { get { return dataLength; } }
		public bool TagAlterPreservation { get { return tagAlterPreservation; } }
		public bool FileAlterPreservation { get { return fileAlterPreservation; } }
		public bool ReadOnly { get { return readOnly; } }
		public bool Compression { get { return compression; } }
		public bool Encryption { get { return encryption; } }
		public bool GroupingIdentity { get { return groupingIdentity; } }
		public int TotalLength { get { return HeaderLength + DataLength; } }

		protected ID3v2Frame(BinaryReader reader)
		{
			this.identifier = Encoding.ASCII.GetString(reader.ReadBytes(4));
			if (!Regex.IsMatch(identifier, "^[A-Z0-9]{4}$")) throw new InvalidDataException(string.Format("Invalid frame identifier '{0}'", identifier));

			this.dataLength = Binary.SwitchEndianness(reader.ReadInt32());

			BitField flags = BitField.FromBytes(reader.ReadBytes(2));
			if (flags[3, 8].Value != 0 || flags[11, 16].Value != 0) throw new InvalidDataException(string.Format("Found unused but set flags '{0}'", flags));

			this.tagAlterPreservation = !flags[0];
			this.fileAlterPreservation = !flags[1];
			this.readOnly = flags[2];
			this.compression = flags[8];
			this.encryption = flags[9];
			this.groupingIdentity = flags[10];

			// TODO:
			if (compression || encryption || groupingIdentity) throw new NotImplementedException();
		}

		public virtual void Write(BinaryWriter writer)
		{
			writer.Write(Encoding.ASCII.GetBytes(identifier));

			writer.Write(Binary.SwitchEndianness(dataLength));

			writer.Write(BitField.FromBits(Enumerables.Create(!tagAlterPreservation, !fileAlterPreservation, readOnly, false, false, false, false, false)).Bytes.Single());
			writer.Write(BitField.FromBits(Enumerables.Create(compression, encryption, groupingIdentity, false, false, false, false, false)).Bytes.Single());
		}

		public override string ToString()
		{
			return identifier;
		}

		public static Encoding GetEncoding(byte encodingID)
		{
			switch (encodingID)
			{
				case 0: return Encoding.ASCII;
				case 1: return Encoding.Unicode;
				default: throw new ArgumentException(string.Format("Unknown encoding identifier '{0}'.", encodingID));
			}
		}
	}
}
