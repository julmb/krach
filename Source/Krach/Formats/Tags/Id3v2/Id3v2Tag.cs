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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Krach.Extensions;

namespace Krach.Formats.Tags.Id3v2
{
	public class Id3v2Tag
	{
		readonly string identifier;
		readonly byte majorVersion;
		readonly byte minorVersion;
		readonly bool unsynchronisation;
		readonly bool extendedHeader;
		readonly bool experimental;
		readonly int dataLength;
		readonly List<Id3v2Frame> frames = new List<Id3v2Frame>();

		public string Identifier { get { return identifier; } }
		public byte MajorVersion { get { return majorVersion; } }
		public byte MinorVersion { get { return minorVersion; } }
		public bool Unsynchronization { get { return unsynchronisation; } }
		public bool ExtendedHeader { get { return extendedHeader; } }
		public bool Experimental { get { return experimental; } }
		public int HeaderLength { get { return 10; } }
		public int DataLength { get { return dataLength; } }
		public int TotalLength { get { return HeaderLength + DataLength; } }
		public IEnumerable<Id3v2Frame> Frames { get { return frames; } }

		public Id3v2Tag(BinaryReader reader)
		{
			this.identifier = Encoding.ASCII.GetString(reader.ReadBytes(3));
			if (identifier != "ID3") throw new ArgumentException(string.Format("Wrong identifier '{0}', should be 'ID3'.", identifier));

			this.majorVersion = reader.ReadByte();
			this.minorVersion = reader.ReadByte();

			// TODO:
			if (majorVersion != 3) throw new NotImplementedException();

			BitField flags = BitField.FromBytes(reader.ReadBytes(1));
			if (flags[3, 8].Value != 0) throw new ArgumentException(string.Format("Found non-used but set flags '{0}'.", flags));

			this.unsynchronisation = flags[0];
			this.extendedHeader = flags[1];
			this.experimental = flags[2];

			// TODO:
			if (unsynchronisation || extendedHeader || experimental) throw new NotImplementedException();

			BitField dataLengthData = BitField.FromBytes(reader.ReadBytes(4));
			if (dataLengthData[0] || dataLengthData[8] || dataLengthData[16] || dataLengthData[24]) throw new ArgumentException(string.Format("Found wrongly set bits in the length field '{0}'.", dataLengthData));
			dataLengthData = BitField.FromBits(Enumerables.Concatenate(dataLengthData[1, 8].Bits, dataLengthData[9, 16].Bits, dataLengthData[17, 24].Bits, dataLengthData[25, 32].Bits));
			this.dataLength = dataLengthData.Value;

			long startPosition = reader.BaseStream.Position;

			while (reader.BaseStream.Position < startPosition + dataLength)
			{
				// Padding has started
				if (reader.PeekChar() == 0) break;

				string frameIdentifier = Encoding.ASCII.GetString(reader.Peek(4));

				Id3v2Frame frame;

				if (frameIdentifier[0] == 'T')
				{
					if (frameIdentifier == "TXXX") frame = new Id3v2UserTextFrame(reader);
					else frame = new Id3v2TextFrame(reader);
				}
				else if (frameIdentifier == "APIC") frame = new Id3v2ImageFrame(reader);
				else frame = new Id3v2GenericFrame(reader);

				frames.Add(frame);
			}

			reader.ReadToPosition(startPosition + dataLength);
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(Encoding.ASCII.GetBytes(identifier));

			writer.Write(majorVersion);
			writer.Write(minorVersion);

			writer.Write(BitField.FromBits(Enumerables.Create(unsynchronisation, extendedHeader, experimental, false, false, false, false, false)).Bytes.Single());

			int length = dataLength;
			writer.Write((byte)(length & 0x7F));
			length >>= 7;
			writer.Write((byte)(length & 0x7F));
			length >>= 7;
			writer.Write((byte)(length & 0x7F));
			length >>= 7;
			writer.Write((byte)(length & 0x7F));

			long startPosition = writer.BaseStream.Position;

			foreach (Id3v2Frame frame in frames) frame.Write(writer);

			while (writer.BaseStream.Position < startPosition + dataLength) writer.Write((byte)0);
		}
	}
}
