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

namespace Krach.Formats.Tags.ID3v2
{
	public class ID3v2Tag
	{
		readonly byte majorVersion;
		readonly byte minorVersion;
		readonly bool unsynchronisation;
		readonly bool extendedHeader;
		readonly bool experimental;
		readonly int dataLength;
		readonly IEnumerable<ID3v2Frame> frames;

		public byte MajorVersion { get { return majorVersion; } }
		public byte MinorVersion { get { return minorVersion; } }
		public bool Unsynchronization { get { return unsynchronisation; } }
		public bool ExtendedHeader { get { return extendedHeader; } }
		public bool Experimental { get { return experimental; } }
		public int HeaderLength { get { return 10; } }
		public int DataLength { get { return dataLength; } }
		public int TotalLength { get { return HeaderLength + DataLength; } }
		public IEnumerable<ID3v2Frame> Frames { get { return frames; } }

		public ID3v2Tag(byte majorVersion, byte minorVersion, bool unsynchronisation, bool extendedHeader, bool experimental, IEnumerable<ID3v2Frame> frames)
		{
			if (majorVersion != 3) throw new NotImplementedException();
			if (unsynchronisation || extendedHeader || experimental) throw new NotImplementedException();
			if (frames == null) throw new ArgumentNullException("frames");

			this.majorVersion = majorVersion;
			this.minorVersion = minorVersion;

			this.unsynchronisation = unsynchronisation;
			this.extendedHeader = extendedHeader;
			this.experimental = experimental;

			this.dataLength = 0;

			this.frames = frames;
		}
		public ID3v2Tag(BinaryReader reader)
		{
			string identifier = Encoding.ASCII.GetString(reader.ReadBytes(3));
			if (identifier != "ID3") throw new ArgumentException(string.Format("Wrong identifier '{0}', should be 'ID3'.", identifier));

			this.majorVersion = reader.ReadByte();
			this.minorVersion = reader.ReadByte();

			if (majorVersion != 3) throw new NotImplementedException();

			BitField flags = BitField.FromBytes(reader.ReadBytes(1));
			if (flags[3, 8].Value != 0) throw new ArgumentException(string.Format("Found non-used but set flags '{0}'.", flags));

			this.unsynchronisation = flags[0];
			this.extendedHeader = flags[1];
			this.experimental = flags[2];

			if (unsynchronisation || extendedHeader || experimental) throw new NotImplementedException();

			BitField dataLengthData = BitField.FromBytes(reader.ReadBytes(4));
			if (dataLengthData[0] || dataLengthData[8] || dataLengthData[16] || dataLengthData[24]) throw new ArgumentException(string.Format("Found wrongly set bits in the length field '{0}'.", dataLengthData));
			dataLengthData = BitField.FromBits
			(
				Enumerables.Concatenate
				(
					dataLengthData[1, 8].Bits,
					dataLengthData[9, 16].Bits,
					dataLengthData[17, 24].Bits,
					dataLengthData[25, 32].Bits
				)
			);
			this.dataLength = dataLengthData.Value;

			long startPosition = reader.BaseStream.Position;

			this.frames = ReadFrames(reader).ToArray();

			reader.ReadToPosition(startPosition + dataLength);
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(Encoding.ASCII.GetBytes("ID3"));

			writer.Write(majorVersion);
			writer.Write(minorVersion);

			writer.Write(BitField.FromBits(Enumerables.Create(unsynchronisation, extendedHeader, experimental, false, false, false, false, false)).Bytes.Single());

			BitField dataLengthData = BitField.FromValue(dataLength, 28);
			dataLengthData = BitField.FromBits
			(
				Enumerables.Concatenate
				(
					Enumerables.Create(false),
					dataLengthData[0, 7].Bits,
					Enumerables.Create(false),
					dataLengthData[7, 14].Bits,
					Enumerables.Create(false),
					dataLengthData[14, 21].Bits,
					Enumerables.Create(false),
					dataLengthData[21, 28].Bits
				)
			);
			writer.Write(dataLengthData.Bytes.ToArray());

			long startPosition = writer.BaseStream.Position;

			foreach (ID3v2Frame frame in frames) frame.Write(writer);

			while (writer.BaseStream.Position < startPosition + dataLength) writer.Write((byte)0);
		}

		IEnumerable<ID3v2Frame> ReadFrames(BinaryReader reader)
		{
			long startPosition = reader.BaseStream.Position;

			while (reader.BaseStream.Position < startPosition + dataLength)
			{
				// Padding has started
				if (reader.PeekChar() == 0) break;

				string frameIdentifier = Encoding.ASCII.GetString(reader.Peek(4));

				if (frameIdentifier[0] == 'T')
				{
					if (frameIdentifier == "TXXX") yield return new ID3v2UserTextFrame(reader);
					else yield return new ID3v2TextFrame(reader);
				}
				else if (frameIdentifier == "APIC") yield return new ID3v2ImageFrame(reader);
				else yield return new ID3v2GenericFrame(reader);
			}
		}
	}
}
